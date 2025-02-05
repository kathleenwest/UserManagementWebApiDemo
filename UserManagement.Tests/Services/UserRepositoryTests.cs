using Microsoft.EntityFrameworkCore;
using Moq;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Tests.Services
{
    /// <summary>
    /// Unit tests for the UserRepository class.
    /// </summary>
    public class UserRepositoryTests
    {
        /// <summary>
        /// Represents the database context used for accessing user-related data.
        /// </summary>
        private readonly UserDbContext _dbContext;

        /// <summary>
        /// Represents the repository used for performing operations on user data.
        /// </summary>
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class.
        /// Sets up the in-memory database and seeds it with sample data.
        /// We utilize an in-memory database for testing purposes. The run-time
        /// implements SQLite that persists, but the tests are not dependent on it.
        /// </summary>
        public UserRepositoryTests()
        {
            // Create options for DbContext to use an in-memory database
            DbContextOptions<UserDbContext> options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Specify the in-memory database name
                .Options;

            // Initialize UserDbContext with the options
            _dbContext = new UserDbContext(options);

            // Seed the database with sample data
            SeedDatabase();

            // Initialize UserRepository with the dbContext
            _userRepository = new UserRepository(_dbContext);
        }

        /// <summary>
        /// Seeds the in-memory database with sample user data.
        /// </summary>
        private void SeedDatabase()
        {
            // Create a list of sample users
            List<User> users = new List<User>
            {
                new User
                        {
                            Id = Guid.NewGuid(), // Generate a new unique identifier
                            FirstName = "John",
                            LastName = "Doe",
                            Email = "john.doe@example.com",
                            DateOfBirth = new DateTime(1990, 1, 1), // Set the date of birth
                            PhoneNumber = "1234567890"
                        },
                new User
                        {
                            Id = Guid.NewGuid(), // Generate a new unique identifier
                            FirstName = "Jane",
                            LastName = "Smith",
                            Email = "jane.smith@example.com",
                            DateOfBirth = new DateTime(1985, 5, 5), // Set the date of birth
                            PhoneNumber = "0987654321"
                        }
            };

            // Add the sample users to the Users DbSet
            _dbContext.Users.AddRange(users);

            // Save the changes to the in-memory database
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tests the CreateUserAsync method to verify that it returns the created user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task CreateUserAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            User newUser = new User
            {
                Id = Guid.NewGuid(), // Generate a new unique identifier
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1), // Set the date of birth
                PhoneNumber = "1234567890"
            };

            // Act
            User createdUser = await _userRepository.CreateUserAsync(newUser);

            // Assert
            Assert.Equal(newUser.Id, createdUser.Id); // Verify that the created user's Id is not empty
            Assert.Equal(newUser.FirstName, createdUser.FirstName); // Verify that the first names match
            Assert.Equal(newUser.LastName, createdUser.LastName); // Verify that the last names match
            Assert.Equal(newUser.Email, createdUser.Email); // Verify that the email addresses match
            Assert.Equal(newUser.PhoneNumber, createdUser.PhoneNumber); // Verify that the  Phone Number match
            Assert.Equal(newUser.DateOfBirth, createdUser.DateOfBirth); // Verify that the date of birth match

            // Act
            User? userInDb = await _dbContext.Users.FindAsync(createdUser.Id); // Find the created user in the database

            // Assert
            Assert.NotNull(userInDb); // Verify that the user is not null
            Assert.Equal(newUser.FirstName, userInDb.FirstName); // Verify that the first names match
            Assert.Equal(newUser.LastName, userInDb.LastName); // Verify that the last names match
            Assert.Equal(newUser.Email, userInDb.Email); // Verify that the email addresses match
            Assert.Equal(newUser.PhoneNumber, userInDb.PhoneNumber); // Verify that the  Phone Number match
            Assert.Equal(newUser.DateOfBirth, userInDb.DateOfBirth); // Verify that the date of birth match
        }

        /// <summary>
        /// Tests if GetUsersAsync retrieves a list of users.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetUsersAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            // See SeedDatabase method for seeding the database

            // Act
            IEnumerable<User> result = await _userRepository.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// Tests if GetUserByIdAsync retrieves a user when they exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User
            {
                Id = userId, // Set the user Id
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                DateOfBirth = new DateTime(1992, 3, 3), // Set the date of birth
                PhoneNumber = "1122334455"
            };

            _dbContext.Users.Add(user); // Add the user to the Users DbSet
            await _dbContext.SaveChangesAsync(); // Save the changes to the in-memory database

            // Act
            User? result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result); // Verify that the result is not null
            Assert.Equal(user.Id, result.Id); // Verify that the user Ids match
            Assert.Equal(user.FirstName, result.FirstName); // Verify that the first names match
            Assert.Equal(user.LastName, result.LastName); // Verify that the last names match
            Assert.Equal(user.Email, result.Email); // Verify that the email addresses match
        }

        /// <summary>
        /// Tests if GetUserByIdAsync returns null when the user does not exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            // Generate a new GUID for the userId, ensuring it will not exist in the repository
            Guid userId = Guid.NewGuid();

            // Act
            // Attempt to retrieve a user from the repository using the generated userId
            User? result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            // Assert that the result is null, indicating the user does not exist
            Assert.Null(result);
        }

        /// <summary>
        /// Tests if UpdateUserAsync successfully updates an existing user.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUpdatedUser_WhenUserExists()
        {
            // Arrange
            // Generate a new GUID for the userId
            Guid userId = Guid.NewGuid();

            // Create an existing user with initial details
            User existingUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };

            // Add the existing user to the database context and save changes
            _dbContext.Users.Add(existingUser);
            await _dbContext.SaveChangesAsync();

            // Create an updated user with the same ID but different details
            User updatedUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };

            // Act
            // Attempt to update the user in the repository with the new details
            User? result = await _userRepository.UpdateUserAsync(updatedUser);

            // Assert
            // Assert that the returned result matches the updated user details
            Assert.Equal(updatedUser.FirstName, result?.FirstName);
            Assert.Equal(updatedUser.LastName, result?.LastName);

            // Retrieve the user from the database and verify it has been updated
            User? userInDb = await _dbContext.Users.FindAsync(userId);
            Assert.NotNull(userInDb);
            Assert.Equal(updatedUser.FirstName, userInDb.FirstName);
            Assert.Equal(updatedUser.LastName, userInDb.LastName);
        }

        /// <summary>
        /// Tests if UpdateUserAsync returns null when the user does not exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            // Generate a new GUID for the userId, ensuring it will not exist in the repository
            Guid userId = Guid.NewGuid();

            // Create an updated user with the non-existent userId
            User updatedUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };

            // Act
            // Attempt to update the user in the repository, which should return null since the user does not exist
            User? result = await _userRepository.UpdateUserAsync(updatedUser);

            // Assert
            // Assert that the result is null, indicating the user does not exist
            Assert.Null(result);
        }

        /// <summary>
        /// Tests if DeleteUserAsync successfully deletes an existing user.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            // Generate a new GUID for the userId
            Guid userId = Guid.NewGuid();

            // Create a user with initial details
            User user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };

            // Add the user to the database context and save changes
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            // Attempt to delete the user from the repository
            bool result = await _userRepository.DeleteUserAsync(userId);

            // Assert
            // Assert that the result is true, indicating the user was successfully deleted
            Assert.True(result);

            // Retrieve the user from the database and verify it has been deleted
            User? userInDb = await _dbContext.Users.FindAsync(userId);
            Assert.Null(userInDb);
        }

        /// <summary>
        /// Tests if DeleteUserAsync returns false when the user does not exist (unhappy path).
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            // Generate a new GUID for the userId, ensuring it will not exist in the repository
            Guid userId = Guid.NewGuid();

            // Act
            // Attempt to delete a user from the repository with the non-existent userId
            bool result = await _userRepository.DeleteUserAsync(userId);

            // Assert
            // Assert that the result is false, indicating the user does not exist
            Assert.False(result);
        }

        /// <summary>
        /// Tests that GetUsersByEmail returns users with the matching email address.
        /// </summary>
        [Fact]
        public async Task GetUsersByEmail_ReturnsUsersWithMatchingEmail()
        {
            // Act
            List<User> result = await _userRepository.GetUsersByEmail("john.doe@example.com");

            // Assert
            Assert.All(result, user => Assert.Equal("john.doe@example.com", user.Email)); // Verify all users have the expected email
        }

        /// <summary>
        /// Tests that GetUsersByEmail returns an empty list when no users match the email address.
        /// </summary>
        [Fact]
        public async Task GetUsersByEmail_ReturnsEmptyListWhenNoMatchingEmail()
        {
            // Act
            List<User> result = await _userRepository.GetUsersByEmail("nonexistent@example.com");

            // Assert
            Assert.Empty(result); // Verify that no users are returned
        }
    }
}