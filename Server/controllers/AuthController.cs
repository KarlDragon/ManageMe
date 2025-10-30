using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public class RegisterDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterLoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = "Registration failed.", errors = result.Errors });

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterLoginDto dto)
        {
            // 1. Attempt sign-in
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: true);

            // 2. Evaluate result
            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
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

    }
}
