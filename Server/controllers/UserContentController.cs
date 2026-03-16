using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;
using System.Security.Claims;
using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace Server.Controllers;

/// <summary>
/// Controller for managing user content operations.
/// Handles CRUD operations for user spending records.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserContentController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserContentContext _context;
    private readonly ISpendingService _spendingService;

    /// <summary>
    /// Initializes a new instance of the UserContentController class.
    /// </summary>
    /// <param name="userManager">Manager for user operations.</param>
    /// <param name="context">Database context for user content.</param>
    /// <param name="spendingService">Service for spending calculations.</param>
    public UserContentController(
        UserManager<IdentityUser> userManager,
        UserContentContext context,
        ISpendingService spendingService)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _spendingService = spendingService ?? throw new ArgumentNullException(nameof(spendingService));
    }

    /// <summary>
    /// Adds a new user content item (spending record) for the authenticated user.
    /// Converts the provided UTC time to the user's local timezone before storing.
    /// </summary>
    /// <param name="dto">The user content data transfer object containing spending details.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [Authorize]
    [HttpPost("addusercontent")]
    public async Task<IActionResult> AddUserContent([FromBody] UserContentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "User not authenticated." });
        }

        try
        {
            // Parse UTC time from client
            if (!DateTimeOffset.TryParse(dto.DateIso, out var utc))
            {
                return BadRequest(new { message = "Invalid date format." });
            }

            // Convert to user's local timezone
            // tzOffsetMinutes is negative for UTC+7, so negate it: -(-420) = +420
            var userLocal = utc.ToOffset(TimeSpan.FromMinutes(-dto.TzOffsetMinutes));

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

            await _context.UserContents.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User content added successfully!", content = newItem });
        }
        catch (Exception ex)
        {
            // Log the exception (assuming logging is configured)
            return StatusCode(500, new { message = "An error occurred while adding user content.", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves spending data for the authenticated user based on the specified time hierarchy.
    /// </summary>
    /// <param name="hierarchy">The time hierarchy filter (daily, monthly, yearly).</param>
    /// <returns>An IActionResult containing the spending response or error.</returns>
    [Authorize]
    [HttpGet("{hierarchy}")]
    public async Task<IActionResult> GetSpending(string hierarchy)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "User not authenticated." });
        }

        try
        {
            var result = await _spendingService.GetSpending(userId, hierarchy);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, new { message = "An error occurred while retrieving spending data.", error = ex.Message });
        }
    }
}