using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.AccountVMs;

public class LoginVM
{
    [Required(ErrorMessage = "Email address is required!")]
    [EmailAddress(ErrorMessage = "Invalid email!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
