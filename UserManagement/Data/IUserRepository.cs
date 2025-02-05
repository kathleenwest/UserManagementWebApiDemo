using UserManagement.Models;

namespace UserManagement.Data
{
    /// <summary>
    /// Interface for user repository that defines methods for user management operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The created user object.</returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of user objects.</returns>
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user object, or null if not found.</returns>
        Task<User?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user object with updated information.</param>
        /// <returns>The updated user object, or null if not found.</returns>
        Task<User?> UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was deleted, false if not found.</returns>
        Task<bool> DeleteUserAsync(Guid id);

        /// <summary>
        /// Retrieves a list of users whose email matches the given email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A list of users with the specified email address.</returns>
        Task<List<User>> GetUsersByEmail(string email);
    }
}
