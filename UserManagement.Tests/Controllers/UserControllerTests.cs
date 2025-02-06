using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.Controllers;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Tests.Controllers
{
    public class UsersControllerTests
    {
        // Mock object for IUserService
        private readonly Mock<IUserService> _mockUserService;
        // Mock object for ILogger<UsersController>
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        // Instance of UsersController being tested
        private readonly UsersController _controller;

        /// <summary>
        /// Initializes a new instance of the UsersControllerTests class.
        /// This constructor sets up the mock dependencies and the controller instance.
        /// </summary>
        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>(); // Initializing the mock object for IUserService
            _mockLogger = new Mock<ILogger<UsersController>>(); // Initializing the mock object for ILogger
            _controller = new UsersController(_mockUserService.Object, _mockLogger.Object); // Creating an instance of UsersController with the mock dependencies
        }

        /// <summary>
        /// Tests the CreateUser method to ensure it returns a 201 Created response when a user is successfully created.
        /// </summary>
        [Fact]
        public async Task CreateUser_ShouldReturnCreatedAtActionResult_WhenUserIsCreated()
        {
            // Arrange
            User newUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                PhoneNumber = "1234567890"
            };

            _mockUserService.Setup(service => service.CreateUserAsync(newUser)).ReturnsAsync(newUser);
            _mockUserService.Setup(service => service.IsEmailUniqueAsync(newUser.Email)).ReturnsAsync(true);

            // Act
            IActionResult result = await _controller.CreateUser(newUser);

            // Assert
            CreatedAtActionResult createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(newUser, createdAtActionResult.Value);
        }

        /// <summary>
        /// Tests the CreateUser method to ensure it returns a 400 Bad Request response when the model state is invalid.
        /// </summary>
        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            User newUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                PhoneNumber = "YYYYYYYYYY"
            };

            // There is a difference between how the model state validation is handled in the API controller
            // versus the unit test environment. When you invoke the controller method directly in the unit test,
            // the model state might not be automatically validated as it would be when the request
            // is processed by the ASP.NET Core runtime. You need to manually set the model state to invalid in your test.
            _controller.ModelState.AddModelError("PhoneNumber", "The PhoneNumber field is required.");

            // Act
            IActionResult result = await _controller.CreateUser(newUser);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Tests if CreateUser returns a BadRequest when the email is not unique.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task CreateUser_EmailNotUnique_ReturnsBadRequest()
        {
            // Arrange
            User user = new User { Email = "duplicate@example.com" };
            _mockUserService.Setup(service => service.IsEmailUniqueAsync(user.Email))
                            .ReturnsAsync(false); // Mock the IsEmailUniqueAsync method to return false

            // Act
            IActionResult result = await _controller.CreateUser(user);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); // Verify the result is a BadRequestObjectResult
            BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Email is already taken by another user.", badRequestResult?.Value); // Verify the error message
        }

        /// <summary>
        /// Tests if CreateUser returns CreatedAtAction when the email is unique.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task CreateUser_EmailIsUnique_ReturnsCreatedAtAction()
        {
            // Arrange
            User user = new User { Email = "unique@example.com" };
            _mockUserService.Setup(service => service.IsEmailUniqueAsync(user.Email))
                            .ReturnsAsync(true); // Mock the IsEmailUniqueAsync method to return true
            _mockUserService.Setup(service => service.CreateUserAsync(user))
                            .ReturnsAsync(user); // Mock the CreateUserAsync method to return the user

            // Act
            IActionResult result = await _controller.CreateUser(user);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result); // Verify the result is a CreatedAtActionResult
            CreatedAtActionResult? createdAtActionResult = result as CreatedAtActionResult;
            Assert.Equal(nameof(_controller.GetUserById), createdAtActionResult?.ActionName); // Verify the action name
            Assert.Equal(user, createdAtActionResult?.Value); // Verify the user object
        }

        // TODO - This is the additional unit test scenario that needs to be implemented
        // Add the additional unit test scenarios ideas
        // CreateUser_ShouldReturnBadRequest_WhenUserModelIsInvalid X each User model property that requires validation

        /// <summary>
        /// Tests the GetUsers method to ensure it returns a list of users with a 200 OK status.
        /// </summary>
        [Fact]
        public async Task GetUsers_ShouldReturnOkObjectResult_WithListOfUsers()
        {
            // Arrange

            // Creating a list of users
            List<User> usersList = new List<User>
            {
                new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = DateTime.Now.AddYears(-18), PhoneNumber = "1234567890" },
                new User { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", DateOfBirth = DateTime.Now.AddYears(-18), PhoneNumber = "0987654321" }
            };

            // Setting up the mock to return the list of users when GetUsersAsync is called
            _mockUserService.Setup(service => service.GetUsersAsync()).ReturnsAsync(usersList);

            // Act
            IActionResult result = await _controller.GetUsers(); // Calling GetUsers method

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result); // Verifying the result type is OkObjectResult
            Assert.Equal(200, okResult.StatusCode); // Asserting the status code is 200 OK
            Assert.Equal(usersList, okResult.Value); // Asserting the returned value is the list of users
        }

        /// <summary>
        /// Tests the GetUserById method to ensure it returns a user with a 200 OK status.
        /// </summary>
        [Fact]
        public async Task GetUserById_ShouldReturnOkObjectResult_WithUser()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();
            // Creating a user object
            User user = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = DateTime.Now.AddYears(-18), PhoneNumber = "1234567890" };

            // Setting up the mock to return the user when GetUserByIdAsync is called with the specific userId
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            IActionResult result = await _controller.GetUserById(userId); // Calling GetUserById method

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result); // Verifying the result type is OkObjectResult
            Assert.Equal(200, okResult.StatusCode); // Asserting the status code is 200 OK
            Assert.Equal(user, okResult.Value); // Asserting the returned value is the user object
        }

        /// <summary>
        /// Tests the GetUserById method to ensure it returns a 404 Not Found response when the user is not found.
        /// </summary>
        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserIsNotFound()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();

            // Setting up the mock to return null when GetUserByIdAsync is called with the specific userId
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            IActionResult result = await _controller.GetUserById(userId); // Calling GetUserById method

            // Assert
            NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result); // Verifying the result type is NotFoundResult
            Assert.Equal(404, notFoundResult.StatusCode); // Asserting the status code is 404 Not Found
        }

        /// <summary>
        /// Tests the UpdateUser method to ensure it returns a 200 OK response when a user is successfully updated.
        /// </summary>
        [Fact]
        public async Task UpdateUser_ShouldReturnOkObjectResult_WhenUserIsUpdated()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();
            // Creating an updated user object
            User updatedUser = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = DateTime.Now.AddYears(-18), PhoneNumber = "1234567890" };

            // Setting up the mock to return the updated user when UpdateUserAsync is called with the specific userId and user object
            _mockUserService.Setup(service => service.UpdateUserAsync(userId, updatedUser)).ReturnsAsync(updatedUser);

            // Setting up the mock to return the updated user when ListUsersSameEmailAsync is called with the specific Email
            _mockUserService.Setup(service => service.ListUsersSameEmailAsync(updatedUser.Email)).ReturnsAsync(new List<User> { updatedUser});

            // Act
            IActionResult result = await _controller.UpdateUser(userId, updatedUser); // Calling UpdateUser method

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result); // Verifying the result type is OkObjectResult
            Assert.Equal(200, okResult.StatusCode); // Asserting the status code is 200 OK
            Assert.Equal(updatedUser, okResult.Value); // Asserting the returned value is the updated user object
        }

        /// <summary>
        /// Tests the UpdateUser method to ensure it returns a 404 Not Found response when the user to update is not found.
        /// </summary>
        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserIsNotFound()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();
            // Creating an updated user object
            User updatedUser = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = DateTime.Now.AddYears(-18), PhoneNumber = "1234567890" };

            // Setting up the mock to return null when UpdateUserAsync is called with the specific userId and user object
            _mockUserService.Setup(service => service.UpdateUserAsync(userId, updatedUser)).ReturnsAsync((User?)null);

            // Setting up the mock to return the updated user when ListUsersSameEmailAsync is called with the specific Email
            _mockUserService.Setup(service => service.ListUsersSameEmailAsync(updatedUser.Email)).ReturnsAsync(new List<User> { updatedUser });

            // Act
            IActionResult result = await _controller.UpdateUser(userId, updatedUser); // Calling UpdateUser method

            // Assert
            NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result); // Verifying the result type is NotFoundResult
            Assert.Equal(404, notFoundResult.StatusCode); // Asserting the status code is 404 Not Found
        }

        // TODO - This is the additional unit test scenario that needs to be implemented
        // Add the additional unit test scenarios ideas
        // UpdateUser_ShouldReturnBadRequest_WhenUserModelIsInvalid X each User model property that requires validation
        // UpdateUser_ShouldReturnBadRequest_WhenEmailIsNotUnique "Email is already taken by another user."

        /// <summary>
        /// Tests the DeleteUser method to ensure it returns a 204 No Content response when a user is successfully deleted.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ShouldReturnNoContentResult_WhenUserIsDeleted()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();

            // Setting up the mock to return true when DeleteUserAsync is called with the specific userId
            _mockUserService.Setup(service => service.DeleteUserAsync(userId)).ReturnsAsync(true);

            // Act
            IActionResult result = await _controller.DeleteUser(userId); // Calling DeleteUser method

            // Assert
            NoContentResult noContentResult = Assert.IsType<NoContentResult>(result); // Verifying the result type is NoContentResult
            Assert.Equal(204, noContentResult.StatusCode); // Asserting the status code is 204 No Content
        }

        /// <summary>
        /// Tests the DeleteUser method to ensure it returns a 404 Not Found response when the user to delete is not found.
        /// </summary>
        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserIsNotFound()
        {
            // Arrange

            // Generating a new Guid for the userId
            Guid userId = Guid.NewGuid();

            // Setting up the mock to return false when DeleteUserAsync is called with the specific userId
            _mockUserService.Setup(service => service.DeleteUserAsync(userId)).ReturnsAsync(false);

            // Act
            IActionResult result = await _controller.DeleteUser(userId); // Calling DeleteUser method

            // Assert
            NotFoundResult notFoundResult = Assert.IsType<NotFoundResult>(result); // Verifying the result type is NotFoundResult
            Assert.Equal(404, notFoundResult.StatusCode); // Asserting the status code is 404 Not Found
        }
    
    }
}
