using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.AccountVMs;

public class RegisterVM
{
    [Required(ErrorMessage = "First Name is required!")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required!")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email address is required!")]
    [EmailAddress(ErrorMessage = "Invalid email!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required!")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}
