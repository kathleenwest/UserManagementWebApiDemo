using UserManagement.Models;

namespace UserManagement.Services
{
    /// <summary>
    /// Interface for user service that defines methods for user management operations.
    /// </summary>
    public interface IUserService
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
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="user">The user object with updated information.</param>
        /// <returns>The updated user object, or null if not found.</returns>
        Task<User?> UpdateUserAsync(Guid id, User user);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was deleted, false if not found.</returns>
        Task<bool> DeleteUserAsync(Guid id);

        /// <summary>
        /// Checks if the provided email address is unique among the users.
        /// </summary>
        /// <param name="email">The email address to check for uniqueness.</param>
        /// <returns>A boolean value indicating whether the email is unique (true) or already exists (false).</returns>
        Task<bool> IsEmailUniqueAsync(string email);

        /// <summary>
        /// Get list of users that share the same e-mail address
        /// </summary>
        /// <param name="email">The email address to check for uniqueness.</param>
        /// <returns>A list of users that share the same e-mail</returns>
        Task<List<User>> ListUsersSameEmailAsync(string email);
    }
}
