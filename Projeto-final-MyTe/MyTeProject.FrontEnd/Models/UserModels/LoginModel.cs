using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.UserModels
{
    public class LoginModel
    { 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{3,}$", ErrorMessage = "The password field must contain at least 3 digits, including letters, numbers and special characters")]
        public string Password { get; set; }
    }
}
