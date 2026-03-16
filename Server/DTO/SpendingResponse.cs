using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace Server.DTO;

/// <summary>
/// Response model for spending data queries.
/// Contains the total spending amount and the list of spending items.
/// </summary>
public class SpendingResponse
{
    /// <summary>
    /// The total amount spent in the specified period.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// The list of individual spending items.
    /// </summary>
    public List<UserContentModel> Items { get; set; } = new();
}