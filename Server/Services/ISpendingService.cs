using Server.DTO;
using UserContent.Models;
namespace Server.Services;

/// <summary>
/// Interface for spending-related services.
/// Defines methods for retrieving spending data.
/// </summary>
public interface ISpendingService
{
    /// <summary>
    /// Retrieves spending information for a user based on time hierarchy.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="hierarchy">The time period hierarchy (daily, monthly, yearly).</param>
    /// <returns>A task containing the spending data with total, byCategory, and byItems.</returns>
    Task<object> GetSpending(string userId, string hierarchy);

    /// <summary>
    /// Modifies an existing user content item with new data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated content information.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of the modification.</returns
    Task<object> ModifyUserContent(UserContentDto dto);
}