using System.ComponentModel.DataAnnotations;

namespace API.Accounts.DTOs;

public class CreateAccountRequest : LoginAccountRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}