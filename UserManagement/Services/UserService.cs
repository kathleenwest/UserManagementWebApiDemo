using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services
{
    /// <summary>
    /// Service layer for user management.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// The user repository for data operations.
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for data operations.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The created user object.</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            // Assign a new unique identifier to the user.
            user.Id = Guid.NewGuid();

            // Create the user using the user repository.
            return await _userRepository.CreateUserAsync(user);
        }

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of user objects.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            // Retrieve all users from the user repository.
            return await _userRepository.GetUsersAsync();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user object, or null if not found.</returns>
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            // Retrieve the user by ID from the user repository.
            return await _userRepository.GetUserByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="user">The user object with updated information.</param>
        /// <returns>The updated user object, or null if not found.</returns>
        public async Task<User?> UpdateUserAsync(Guid id, User user)
        {
            // Find the existing user by their ID in the user repository.
            User? existingUser = await _userRepository.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                return null;
            }

            // Preserve the existing user ID.
            user.Id = existingUser.Id;

            // Update the user using the user repository.
            return await _userRepository.UpdateUserAsync(user);
        }

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was deleted, false if not found.</returns>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            // Delete the user using the user repository.
            return await _userRepository.DeleteUserAsync(id);
        }

        /// <summary>
        /// Checks if the provided email address is unique among the users.
        /// </summary>
        /// <param name="email">The email address to check for uniqueness.</param>
        /// <returns>A boolean value indicating whether the email is unique (true) or already exists (false).</returns>
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            // Retrieve a list of users with the specified email address.
            List<User> usersWithEmail = await _userRepository.GetUsersByEmail(email);

            // Return true if no users were found with the same email address, otherwise false.
            return !usersWithEmail.Any();
        }

        /// <summary>
        /// Get list of users that share the same e-mail address
        /// </summary>
        /// <param name="email">The email address to check for uniqueness.</param>
        /// <returns>A list of users that share the same e-mail</returns>
        public async Task<List<User>> ListUsersSameEmailAsync(string email)
        {
            // Retrieve a list of users with the specified email address.
            List<User> usersWithSameEmail = await _userRepository.GetUsersByEmail(email);

            // Return a list of users that share the same e-mail address
            return usersWithSameEmail;
        }
    }
}