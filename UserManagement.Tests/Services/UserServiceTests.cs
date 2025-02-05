using Moq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Tests.Services
{
    /// <summary>
    /// Unit tests for the UserService class.
    /// </summary>
    public class UserServiceTests
    {
        /// <summary>
        /// Mock of the IUserRepository interface. Used for unit testing purposes.
        /// </summary>
        private readonly Mock<IUserRepository> _mockUserRepository;

        /// <summary>
        /// Instance of the UserService class. Provides operations related to user management.
        /// </summary>
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the UserServiceTests class.
        /// </summary>
        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        /// <summary>
        /// Tests if CreateUserAsync successfully creates a user.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task CreateUserAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            User newUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };
            _mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            // Act
            User createdUser = await _userService.CreateUserAsync(newUser);

            // Assert
            Assert.Equal(newUser, createdUser);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        /// <summary>
        /// Tests if GetUsersAsync retrieves a list of users.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetUsersAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            List<User> users = new List<User>
            {
                new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(1990, 1, 1), PhoneNumber = "1234567890" },
                new User { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DateOfBirth = new DateTime(1992, 2, 2), PhoneNumber = "0987654321" }
            };
            _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);

            // Act
            IEnumerable<User> result = await _userService.GetUsersAsync();

            // Assert
            Assert.Equal(users, result);
            _mockUserRepository.Verify(repo => repo.GetUsersAsync(), Times.Once);
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
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            User? result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        /// <summary>
        /// Tests if GetUserByIdAsync returns null when the user does not exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            User? result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        /// <summary>
        /// Tests if UpdateUserAsync successfully updates an existing user.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUpdatedUser_WhenUserExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User existingUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };
            User updatedUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(updatedUser)).ReturnsAsync(updatedUser);

            // Act
            User? result = await _userService.UpdateUserAsync(userId, updatedUser);

            // Assert
            Assert.Equal(updatedUser, result);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(updatedUser), Times.Once);
        }

        /// <summary>
        /// Tests if UpdateUserAsync returns null when the user does not exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task UpdateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User updatedUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890"
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            User? result = await _userService.UpdateUserAsync(userId, updatedUser);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        }

        /// <summary>
        /// Tests if DeleteUserAsync successfully deletes an existing user.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).ReturnsAsync(true);

            // Act
            bool result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }

        /// <summary>
        /// Tests if DeleteUserAsync returns false when the user does not exist.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).ReturnsAsync(false);

            // Act
            bool result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }
    }
}