using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace UserContent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserContentController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserContentContext _context;

        public UserContentController(UserManager<IdentityUser> userManager, UserContentContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        [HttpPost("addusercontent")]
        public async Task<IActionResult> AddUserContent([FromBody] UserContentDto dto)
        {
            var userEmail = User?.Identity?.Name;
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            // Parse UTC time from client
            var utc = DateTimeOffset.Parse(dto.DateIso);

            // Convert to user's local timezone
            // tzOffsetMinutes is negative for UTC+7, so negate it: -(-420) = +420
            var userLocal = utc.ToOffset(TimeSpan.FromMinutes(-dto.TzOffsetMinutes));

            Console.WriteLine($"Received: {dto.Category}, {dto.Amount}, UTC: {utc}, Local: {userLocal}");
            // Create model entity to store in database
            var newItem = new UserContentModel
            {
                UserId = userId,
                Category = dto.Category,
                MoneySpent = dto.Amount,
                Note = dto.Note,
                Date = userLocal,  // Store the user's local time
                DateIso = dto.DateIso,  // Store UTC for reference
                TzOffsetMinutes = dto.TzOffsetMinutes  // Store offset for future conversions
            };

            Console.WriteLine($"Storing: {newItem.Category}, {newItem.MoneySpent}, Local: {newItem.Date}");

            _context.UserContents.Add(newItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User content added successfully!", content = newItem });
        }

        [Authorize]
        [HttpGet("spending/{hierarchy}")]
        public async Task<IActionResult> GetSpending(string hierarchy)
        {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            // Step 1: Load the user's timezone offset
            // We assume the latest record has the most accurate timezone offset
            var latest = await _context.UserContents
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            // If no records yet → use UTC as fallback
            var offsetMinutes = latest?.TzOffsetMinutes ?? 0;

            // Convert current UTC time -> user's local time
            var userNow = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromMinutes(-offsetMinutes));
            var userToday = userNow.Date; // user's local day

            // Step 2: Base query (SQL)
            var query = _context.UserContents.Where(x => x.UserId == userId);

            switch (hierarchy.ToLower())
            {
                case "daily":
                    query = query.Where(x => x.Date.Date == userToday);
                    break;

                case "monthly":
                    query = query.Where(x =>
                        x.Date.Year == userToday.Year &&
                        x.Date.Month == userToday.Month);
                    break;

                case "yearly":
                    query = query.Where(x => x.Date.Year == userToday.Year);
                    break;

                default:
                    return BadRequest("Invalid hierarchy. Use: daily, monthly, yearly.");
            }

            // Step 3: Query DB totals
            var items = await query.ToListAsync();

            var total = items.Sum(x => x.MoneySpent);

            var byCategory = items
                .GroupBy(x => x.Category)
                .Select(g => new { category = g.Key, amount = g.Sum(y => y.MoneySpent) })
                .ToList();

            return Ok(new { total, byCategory, items });
        }

    }
}