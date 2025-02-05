using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    /// <summary>
    /// Custom validation attribute to validate the minimum age requirement.
    /// </summary>
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumAgeAttribute"/> class.
        /// </summary>
        /// <param name="minimumAge">The minimum age required.</param>
        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="ValidationResult"/> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                // Calculate the age of the user.
                int age = DateTime.Now.Year - dateOfBirth.Year;
                // Adjust the age if the user hasn't had their birthday yet this year.
                if (DateTime.Now < dateOfBirth.AddYears(age)) age--;

                // Check if the user's age is less than the minimum age required.
                if (age < _minimumAge)
                {
                    // Return a validation error if the user doesn't meet the minimum age requirement.
                    return new ValidationResult($"User must be at least {_minimumAge} years old.");
                }
            }

            // Return success if the validation passes.
            return ValidationResult.Success;
        }
    }
}