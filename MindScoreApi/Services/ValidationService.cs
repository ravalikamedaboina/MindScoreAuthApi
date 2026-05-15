using System.Text.RegularExpressions;
using System.Net.Mail;

namespace MindScoreApi.Services
{
    public interface IValidationService
    {
        ValidationResult ValidateRegisterRequest(string name, string email, string password, int age, string gender);
        ValidationResult ValidateLoginRequest(string email, string password);
        bool IsValidEmail(string email);
    }

    public class ValidationService : IValidationService
    {
        public ValidationResult ValidateRegisterRequest(string name, string email, string password, int age, string gender)
        {
            // Validate empty fields
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(gender) ||
                age == 0)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "All fields are required and cannot be empty."
                };
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Invalid email format."
                };
            }

            // Validate name contains only letters and spaces
            if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Name must contain only letters and spaces, no numbers."
                };
            }

            // Validate age is a valid number
            if (age <= 0 || age > 150)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Age must be a valid number between 1 and 150."
                };
            }

            return new ValidationResult { IsValid = true };
        }

        public ValidationResult ValidateLoginRequest(string email, string password)
        {
            // Validate empty fields
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Email and password are required."
                };
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Invalid email format."
                };
            }

            return new ValidationResult { IsValid = true };
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}