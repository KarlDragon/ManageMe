using Microsoft.EntityFrameworkCore;
using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace Server.Repositories;

/// <summary>
/// Implementation of IUserContentRepository using Entity Framework Core.
/// Handles data access operations for UserContent entities.
/// </summary>
public class UserContentRepository : IUserContentRepository
{
    private readonly UserContentContext _context;

    /// <summary>
    /// Initializes a new instance of the UserContentRepository class.
    /// </summary>
    /// <param name="context">The database context for user content.</param>
    public UserContentRepository(UserContentContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Adds a new user content item to the database asynchronously.
    /// </summary>
    /// <param name="userContent">The user content entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync(UserContentModel userContent)
    {
        await _context.UserContents.AddAsync(userContent);
    }

    /// <summary>
    /// Retrieves user content items filtered by user ID and time hierarchy.
    /// Supports daily, monthly, and yearly filtering based on UTC time.
    /// </summary>
    /// <param name="userId">The ID of the user whose content to retrieve.</param>
    /// <param name="hierarchy">The time hierarchy: "daily", "monthly", or "yearly".</param>
    /// <returns>A list of user content items matching the filter criteria.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid hierarchy is provided.</exception>
    public async Task<List<UserContentModel>> GetByUserAndHierarchyAsync(string userId, string hierarchy)
    {
        var query = _context.UserContents.Where(x => x.UserId == userId);

        var today = DateTime.UtcNow;
        
        Console.WriteLine($"Current UTC time: {today:yyyy-MM-dd HH:mm:ss}");
        switch (hierarchy.ToLower())
        {
            case "daily":
                query = query.Where(x => x.Date.Date == today.Date);
                break;
            case "monthly":
                query = query.Where(x => x.Date.Month == today.Month && x.Date.Year == today.Year);
                break;
            case "yearly":
                query = query.Where(x => x.Date.Year == today.Year);
                break;
            default:
                throw new ArgumentException("Invalid hierarchy. Must be 'daily', 'monthly', or 'yearly'.", nameof(hierarchy));
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Modifies an existing user content item in the database.
    /// The item is identified by its ID, and the provided DTO contains the updated values.
    /// </summary>
    /// <param name="userContentDto">The user content entity with updated values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ModifyAsync(UserContentDto userContentDto)
    {
        // Find the existing item by ID 
        var existingItem = await _context.UserContents.FindAsync(userContentDto.Id);

        if (existingItem == null)
        {
            throw new KeyNotFoundException($"User content with ID {userContentDto.Id} not found.");
        }
        
        DateTimeOffset.TryParse(userContentDto.DateIso, out var utcDate);
        // Update properties
        existingItem.Category = userContentDto.Category;
        existingItem.MoneySpent = userContentDto.MoneySpent;
        existingItem.Note = userContentDto.Note;
        existingItem.Date = utcDate;

        _context.UserContents.Update(existingItem);

        await Task.CompletedTask;
    }

    public Task ModifyAsync(UserContentModel userContentDto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a user content item from the database based on its ID.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existingItem = _context.UserContents.Find(id);

        if (existingItem == null)
        {
            throw new KeyNotFoundException($"User content with ID {id} not found.");
        }

        _context.UserContents.Remove(existingItem);

        await Task.CompletedTask;

    }

    public Task DeleteAsync(UserContentModel userContentDto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}