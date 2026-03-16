using System.ComponentModel.DataAnnotations;

namespace Server.Models;

/// <summary>
/// Data Transfer Object for user registration.
/// Contains the necessary information to create a new user account.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The email address of the user. Must be a valid email format.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password for the user account. Must meet minimum requirements.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for user login.
/// Contains credentials and login preferences.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// The email address of the user attempting to log in.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password for the user account.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether to remember the user's login session.
    /// </summary>
    public bool RememberMe { get; set; }
}