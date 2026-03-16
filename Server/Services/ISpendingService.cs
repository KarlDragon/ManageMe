using Server.DTO;

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
    /// <returns>A task containing the spending response with total and items.</returns>
    Task<SpendingResponse> GetSpending(string userId, string hierarchy);
}