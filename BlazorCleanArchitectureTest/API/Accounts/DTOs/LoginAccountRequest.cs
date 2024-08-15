using System.ComponentModel.DataAnnotations;

namespace API.Accounts.DTOs;

public class LoginAccountRequest
{
    [EmailAddress, Required]
    [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Your email is not valid, provide a valid email")]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]+$", ErrorMessage = "Your password must be a mix of Alphanumeric and special characters")]
    public string Password { get; set; } = string.Empty;
}