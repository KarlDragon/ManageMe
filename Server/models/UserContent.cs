using System.ComponentModel.DataAnnotations;

namespace UserContent.Models;

/// <summary>
/// Entity model representing a user's spending content.
/// Stored in the database with user-specific spending records.
/// </summary>
public class UserContent
{
    /// <summary>
    /// The unique identifier for this spending record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user who made this spending record.
    /// Links to the Identity user.
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The category of the spending (e.g., "Food", "Transport").
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The amount of money spent.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public int MoneySpent { get; set; }

    /// <summary>
    /// Optional note or description for the spending.
    /// </summary>
    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// The date and time of the spending in the user's local timezone.
    /// </summary>
    [Required]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// The date and time in ISO 8601 format (UTC).
    /// Stored for reference and potential future conversions.
    /// </summary>
    [Required]
    public string DateIso { get; set; } = string.Empty;

    /// <summary>
    /// The timezone offset in minutes from UTC.
    /// Used to convert between UTC and user's local time.
    /// </summary>
    public int TzOffsetMinutes { get; set; }
}

/// <summary>
/// Data Transfer Object for receiving user content from the client.
/// Used for creating new spending records.
/// </summary>
public class UserContentDto
{
    /// <summary>
    /// The category of the spending.
    /// </summary>
    [Required(ErrorMessage = "Category is required.")]
    [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The amount spent.
    /// </summary>
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public int Amount { get; set; }

    /// <summary>
    /// Optional note for the spending.
    /// </summary>
    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// The date in ISO 8601 format (UTC).
    /// </summary>
    [Required(ErrorMessage = "Date is required.")]
    public string DateIso { get; set; } = string.Empty;

    /// <summary>
    /// The timezone offset in minutes.
    /// </summary>
    [Required(ErrorMessage = "Timezone offset is required.")]
    public int TzOffsetMinutes { get; set; }
}