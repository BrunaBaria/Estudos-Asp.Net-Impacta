using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.UserModels
{
    public class DepartmentModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "The email isn't valid.")]
        public string? ContactEmail { get; set; }

        public int? QuantityOfEmployees { get; set; }
    }
}
