using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.UserModels
{
    public class HiringRegimeModel
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public bool AcceptOvertime { get; set; }

        [Required(ErrorMessage = "The field WorkSchedule is necessary.")]
        [Range(1, 24, ErrorMessage = "The WorkSchedule must be between 1 and 24 hours.")]
        public decimal WorkSchedule { get; set; }
    }
}
