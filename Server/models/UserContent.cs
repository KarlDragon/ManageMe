namespace UserContent.Models;
public class UserContent
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MoneySpent { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    public string DateIso { get; set; } = string.Empty;
    public int TzOffsetMinutes { get; set; }
}

// Separate DTO for receiving data from client
public class UserContentDto
{
    public string Category { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public string DateIso { get; set; } = string.Empty;
    public int TzOffsetMinutes { get; set; }
}