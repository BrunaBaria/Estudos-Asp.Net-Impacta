using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.UserModels
{
    public class LocationModel
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(2)]
        public string? State { get; set; }

        [Required]
        public string? City { get; set; }

        public int? QuantityOfEmployees { get; set; }
    }
}
