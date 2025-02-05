using Microsoft.EntityFrameworkCore;

namespace UserManagement.Models
{
    /// <summary>
    /// Database context for the User Management application.
    /// </summary>
    public class UserDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="options">Options for configuring the database context.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet representing the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}