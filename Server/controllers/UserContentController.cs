using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}