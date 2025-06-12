using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Socializer.Shared.Dtos;

public class CreateUserDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, Password]
    public string Password { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    public string Username { get; set; }
}

public class PasswordAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password))
            return new ValidationResult("Password is required.");

        if (password.Length < 8)
            return new ValidationResult("Password must be at least 8 characters long.");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            return new ValidationResult("Password must contain at least one uppercase letter.");

        if (!Regex.IsMatch(password, @"[a-z]"))
            return new ValidationResult("Password must contain at least one lowercase letter.");

        if (!Regex.IsMatch(password, @"[0-9]"))
            return new ValidationResult("Password must contain at least one number.");

        if (!Regex.IsMatch(password, @"[\W_]"))
            return new ValidationResult("Password must contain at least one special character.");

        return ValidationResult.Success!;
    }
}
