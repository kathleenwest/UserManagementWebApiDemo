using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    /// <summary>
    /// Simple User Model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [MaxLength(128)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [MinimumAge(18)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        [Required]
        [Phone]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits long.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets the age of the user calculated from the date of birth.
        /// </summary>
        public int Age
        {
            get
            {
                // Calculate the age of the user.
                int age = DateTime.Now.Year - DateOfBirth.Year;
                // Adjust the age if the user hasn't had their birthday yet this year.
                if (DateTime.Now < DateOfBirth.AddYears(age)) age--;
                return age;
            }
        }

        /// <summary>
        /// Returns a string representation of the user.
        /// </summary>
        /// <returns>A string containing the user details.</returns>
        public override string ToString()
        {
            // Format the phone number as (XXX) XXX-XXXX.
            string formattedPhoneNumber = String.Format("({0}) {1}-{2}", PhoneNumber.Substring(0, 3), PhoneNumber.Substring(3, 3), PhoneNumber.Substring(6, 4));

            // Return the user details as a formatted string.
            return $"Id: {Id}\nName: {FirstName} {LastName}\nEmail: {Email}\nDate of Birth: {DateOfBirth.ToShortDateString()}\nAge: {Age}\nPhone Number: {formattedPhoneNumber}";
        }

        /// <summary>
        /// Returns a list of user details as strings.
        /// </summary>
        /// <param name="users">The list of users.</param>
        /// <returns>A list of strings containing user details.</returns>
        public static List<string> GetUsers(List<User> users)
        {
            // Initialize a list to store user details.
            List<string> userDetailsList = new List<string>();
            // Iterate through the list of users and add their details to the list.
            foreach (User user in users)
            {
                userDetailsList.Add(user.ToString());
            }
            // Return the list of user details.
            return userDetailsList;
        }

    } // end of class

} // end of namespace