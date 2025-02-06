using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    /// <summary>
    /// Web API actions for User Management application.
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Service for user management operations.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Logger for logging information.
        /// </summary>
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service for user management operations.</param>
        /// <param name="logger">Logger for logging information.</param>
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user.
        /// POST /api/users
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The created user object.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            // Check if the model state is valid.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is unique.
            bool isEmailUnique = await _userService.IsEmailUniqueAsync(user.Email);
            if (!isEmailUnique)
            {
                return BadRequest("Email is already taken by another user.");
            }

            // Create the user using the user service.
            User? createdUser = await _userService.CreateUserAsync(user);

            // Return the created user with a 201 Created status.
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// GET /api/users
        /// </summary>
        /// <returns>A list of user objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // Get the list of users from the user service.
            IEnumerable<User> users = await _userService.GetUsersAsync();

            // Return the list of users with a 200 OK status.
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// GET /api/users/{id}
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user object.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            // Get the user by ID from the user service.
            User? user = await _userService.GetUserByIdAsync(id);

            // Check if the user was not found.
            if (user == null)
            {
                return NotFound();
            }

            // Return the user object with a 200 OK status.
            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user.
        /// PUT /api/users/{id}
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="user">The user object with updated information.</param>
        /// <returns>The updated user object.</returns> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            // Check if the model state is valid.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the list of users with the same email address.
            List<User> users = await _userService.ListUsersSameEmailAsync(user.Email);

            // Check if the incoming email is unique and matched to this user
            foreach (User u in users)
            {
                if (u.Id != id)
                {
                    return BadRequest("Email is already taken by another user.");
                }
            }

            // Update the user using the user service.
            User? updatedUser = await _userService.UpdateUserAsync(id, user);

            // Check if the user was not found.
            if (updatedUser == null)
            {
                return NotFound();
            }

            // Return the updated user object with a 200 OK status.
            return Ok(updatedUser);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// DELETE /api/users/{id}
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>No content if the user was deleted.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            // Delete the user using the user service.
            bool result = await _userService.DeleteUserAsync(id);

            // Check if the user was not found.
            if (!result)
            {
                return NotFound();
            }

            // Return no content with a 204 No Content status.
            return NoContent();
        }
    }
}