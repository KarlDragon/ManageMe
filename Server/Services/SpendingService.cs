using Microsoft.Extensions.Logging;
using Server.DTO;
using Server.Repositories;
using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace Server.Services;

/// <summary>
/// Service for handling spending-related business logic.
/// Implements ISpendingService to provide spending data calculations.
/// </summary>
public class SpendingService : ISpendingService
{
    private readonly IUserContentRepository _userContentRepository;
    private readonly ILogger<SpendingService> _logger;

    /// <summary>
    /// Initializes a new instance of the SpendingService class.
    /// </summary>
    /// <param name="userContentRepository">Repository for accessing user content data.</param>
    /// <param name="logger">Logger for recording service operations.</param>
    public SpendingService(IUserContentRepository userContentRepository, ILogger<SpendingService> logger)
    {
        _userContentRepository = userContentRepository ?? throw new ArgumentNullException(nameof(userContentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves spending data for a user based on the specified time hierarchy.
    /// Calculates total spending and returns detailed items.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="hierarchy">The time hierarchy filter (daily, monthly, yearly).</param>
    /// <returns>A SpendingResponse containing total and item details.</returns>
    /// <exception cref="ArgumentException">Thrown when hierarchy is invalid.</exception>
    public async Task<SpendingResponse> GetSpending(string userId, string hierarchy)
    {
        _logger.LogInformation("Retrieving spending data for user {UserId} with hierarchy {Hierarchy}", userId, hierarchy);

        try
        {
            var items = await _userContentRepository.GetByUserAndHierarchyAsync(userId, hierarchy);

            var total = items.Sum(x => (decimal)x.MoneySpent);

            _logger.LogInformation("Retrieved {ItemCount} items with total spending {Total} for user {UserId}",
                items.Count, total, userId);

            return new SpendingResponse
            {
                Total = total,
                Items = items
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving spending data for user {UserId}", userId);
            throw;
        }
    }
}