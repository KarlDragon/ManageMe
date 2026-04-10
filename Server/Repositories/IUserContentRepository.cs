using UserContent.Models;
using UserContentModel = UserContent.Models.UserContent;

namespace Server.Repositories;

/// <summary>
/// Interface for managing user content data operations.
/// This follows the Repository pattern to abstract data access logic.
/// </summary>
public interface IUserContentRepository
{
    /// <summary>
    /// Adds a new user content item to the database.
    /// </summary>
    /// <param name="userContent">The user content entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(UserContentModel userContent);

    /// <summary>
    /// Retrieves user content items based on user ID and date hierarchy.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="hierarchy">The time hierarchy filter (daily, monthly, yearly).</param>
    /// <returns>A list of user content items matching the criteria.</returns>
    Task<List<UserContentModel>> GetByUserAndHierarchyAsync(string userId, string hierarchy);

    /// <summary>
    /// Modifies an existing user content item in the database.
    /// </summary>
    /// <param name="userContentDto">The user content entity with updated values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ModifyAsync(UserContentDto userContentDto);

    /// <summary>
    /// Deletes a user content item from the database based on its ID.
    /// </summary>
    /// <param name="id">The ID of the user content item to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    
    Task<object> DeleteAsync(int id);

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveChangesAsync();
}