using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.ExpenseModels
{
    public class ExpenseModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The expense value must be greater than zero")]
        public double Value { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? WBSDescription { get; set; }

        [Required]
        public int? WBSId { get; set; }

        public string? ExpenseTypeDescription { get; set; }

        [Required]
        public int? ExpenseTypeId { get; set; }
    }
}
