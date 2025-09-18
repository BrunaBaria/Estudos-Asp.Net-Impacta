using MyTeProject.FrontEnd.Utils.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.UserModels
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        #region User Data

        [Required]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "The Name field must be composed of letters, contain no blank spaces and no special characters")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{3,}$", ErrorMessage = "The password field must contain at least 3 digits, including letters, numbers and special characters")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match!")]
        public string PasswordConfirm { get; set; }

        [Required]
        public bool Active { get; set; } = true;

        [Required]

        public DateTime AdmissionDate { get; set; }

        [Required]
        public EnumRole? Role { get; set; }

        #endregion User Data

        #region Hiring Regime

        [Required]
        public int? HiringRegimeId { get; set; }

        public string? HiringRegime { get; set; }

        public bool? AcceptOvertime { get; set; }
        
        public decimal? WorkSchedule {  get; set; }

        #endregion Hiring Regime

        #region Department

        [Required]
        public int? DepartmentId { get; set; }

        public string? Department { get; set; }

        #endregion Department

        #region Location

        [Required]
        public int? LocationId { get; set; }
        public string? Location { get; set; }

        #endregion Location

    }
}
