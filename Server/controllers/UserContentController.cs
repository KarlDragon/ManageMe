using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;
using System.Security.Claims;
using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;
using Server.Repositories;
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
    private readonly IUserContentRepository _userContentRepository;
    /// <summary>
    /// Initializes a new instance of the UserContentController class.
    /// </summary>
    /// <param name="userManager">Manager for user operations.</param>
    /// <param name="context">Database context for user content.</param>
    /// <param name="spendingService">Service for spending calculations.</param>
    public UserContentController(
        UserManager<IdentityUser> userManager,
        UserContentContext context,
        ISpendingService spendingService,
        IUserContentRepository userContentRepository)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _spendingService = spendingService ?? throw new ArgumentNullException(nameof(spendingService));
        _userContentRepository = userContentRepository ?? throw new ArgumentNullException(nameof(userContentRepository));
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
            if (!DateTimeOffset.TryParse(dto.DateIso, out var utcDate))
            {
                return BadRequest(new { message = "Invalid date format." });
            }

            // Create model entity to store in database
            var newItem = new UserContentModel
            {
                UserId = userId,
                Category = dto.Category,
                MoneySpent = dto.MoneySpent,
                Note = dto.Note,
                Date = utcDate,
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

    [Authorize]
    [HttpPut("modifyusercontent")]
    public async Task<IActionResult> EditUserContent([FromBody] UserContentDto dto)
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
            await _userContentRepository.ModifyAsync(dto);

            await _userContentRepository.SaveChangesAsync();

            return Ok(new { message = "User content modified successfully!" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception (assuming logging is configured)
            return StatusCode(500, new { message = "An error occurred while modifying user content.", error = ex.Message });
    }
    }




    /// <summary>
    /// Retrieves spending data for the authenticated user based on the specified time hierarchy.
    /// </summary>
    /// <param name="hierarchy">The time hierarchy filter (daily, monthly, yearly).</param>
    /// <returns>An IActionResult containing the spending response or error.</returns>
    [Authorize]
    [HttpGet("spending/{hierarchy}")]
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