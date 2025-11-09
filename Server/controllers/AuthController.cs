using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenticated = true, user = User.Identity.Name });
            }
            else
            {
                return Ok(new { isAuthenticated = false });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = "Registration failed.", errors = result.Errors });

            return Ok(new { message = "User registered successfully!" , Email = user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1. Attempt sign-in
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: dto.RememberMe, lockoutOnFailure: true);
            Console.WriteLine($"Login attempt for {dto.Email}: Succeeded={result.Succeeded}, IsLockedOut={result.IsLockedOut}, IsNotAllowed={result.IsNotAllowed}, isPersistent={dto.RememberMe}");
            // 2. Evaluate result
            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful", Email = dto.Email });
            }
            else if (result.IsLockedOut)
            {
                return Forbid("Account is locked. Try again later.");
            }
            else if (result.IsNotAllowed)
            {
                return Unauthorized(new { message = "Login not allowed (e.g. email not confirmed)." });
            }
            else
            {
                return Unauthorized(new { message = "Invalid login attempt." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }

        
    }
}
