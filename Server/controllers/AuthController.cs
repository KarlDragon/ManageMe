using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace Server.Controllers;

/// <summary>
/// Controller for handling authentication operations.
/// Manages user registration, login, logout, and authentication status.
/// </summary>
[ApiController]
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the AuthController class.
    /// </summary>
    /// <param name="userManager">Manager for user operations.</param>
    /// <param name="signInManager">Manager for sign-in operations.</param>
    /// <param name="logger">Logger for recording authentication operations.</param>
    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AuthController> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the authentication status of the current user.
    /// </summary>
    /// <returns>An IActionResult indicating whether the user is authenticated.</returns>
    [HttpGet("status")]
    public IActionResult Status()
    {
        if (User?.Identity != null && User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("User {UserName} is authenticated.", User.Identity.Name);
            return Ok(new { isAuthenticated = true, user = User.Identity.Name });
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
            return Ok(new { isAuthenticated = false });
        }
    }

    /// <summary>
    /// Registers a new user with the provided credentials.
    /// </summary>
    /// <param name="dto">The registration data containing email and password.</param>
    /// <returns>An IActionResult indicating success or failure of registration.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Attempting to register user with email {Email}", dto.Email);

        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Registration failed for {Email}: {Errors}",
                dto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(new { message = "Registration failed.", errors = result.Errors });
        }

        _logger.LogInformation("User {Email} registered successfully.", dto.Email);
        return Ok(new { user = user.Email });
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="dto">The login data containing email, password, and remember me option.</param>
    /// <returns>An IActionResult indicating success or failure of login.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Login attempt for {Email}", dto.Email);

        // Attempt sign-in
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: true);

        // Evaluate result
        if (result.Succeeded)
        {
            _logger.LogInformation("Login successful for {Email}", dto.Email);
            return Ok(new { user = dto.Email });
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("Account locked out for {Email}", dto.Email);
            return Forbid("Account is locked. Try again later.");
        }
        else if (result.IsNotAllowed)
        {
            _logger.LogWarning("Login not allowed for {Email} (e.g., email not confirmed)", dto.Email);
            return Unauthorized(new { message = "Login not allowed (e.g. email not confirmed)." });
        }
        else
        {
            _logger.LogWarning("Invalid login attempt for {Email}", dto.Email);
            return Unauthorized(new { message = "Invalid login attempt." });
        }
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>An IActionResult indicating successful logout.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("User logging out.");
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out successfully.");
        return Ok(new { message = "Logged out successfully." });
    }
}
