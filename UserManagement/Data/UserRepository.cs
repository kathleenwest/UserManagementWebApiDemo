using Microsoft.EntityFrameworkCore;
using Moq;
using UserManagement.Models;
using Xunit;

namespace UserManagement.Data
{
    /// <summary>
    /// Repository for managing user data.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The database context for user data.
        /// </summary>
        private readonly UserDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for user data.</param>
        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
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

            // Add the user to the DbContext.
            _dbContext.Users.Add(user);

            // Save changes to the database asynchronously.
            await _dbContext.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of user objects.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            // Retrieve all users from the database asynchronously.
            return await _dbContext.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user object, or null if not found.</returns>
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            // Find the user by their ID in the database asynchronously.
            return await _dbContext.Users.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user object with updated information.</param>
        /// <returns>The updated user object, or null if not found.</returns>
        public async Task<User?> UpdateUserAsync(User user)
        {
            // Find the existing user by their ID in the database.
            User? existingUser = await _dbContext.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                return null;
            }

            // Update the user details.
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.PhoneNumber = user.PhoneNumber;

            // Update the user in the DbContext.
            _dbContext.Users.Update(existingUser);

            // Save changes to the database asynchronously.
            await _dbContext.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was deleted, false if not found.</returns>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            // Find the user by their ID in the database.
            User? user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            // Remove the user from the DbContext.
            _dbContext.Users.Remove(user);

            // Save changes to the database asynchronously.
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves a list of users whose email matches the given email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A list of users with the specified email address.</returns>
        public async Task<List<User>> GetUsersByEmail(string email)
        {
            // Perform a LINQ query on the Users DbSet to find users with the specified email address
            return await _dbContext.Users
                             .Where(user => user.Email == email) // Filter users by email
                             .ToListAsync(); // Convert the query result to a list
        }
    }
}