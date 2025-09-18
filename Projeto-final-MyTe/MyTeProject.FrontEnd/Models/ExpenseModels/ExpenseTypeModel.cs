using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.ExpenseModels
{
    public class ExpenseTypeModel
    {
        public int Id { get; set; }

        [Required]

        public string? Description { get; set; }
    }
}
