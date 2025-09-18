using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyTeProject.BackEnd.Entities.ExpenseEntities
{
    public class ExpenseType
    {
        [Key]
        [Column("ExpenseType_Id")]
        public int Id { get; set; }

        [Required]
        [Column("ExpenseType_Description")]
        public string? Description { get; set; }

        public IEnumerable<Expense>? Expenses { get; set; }
    }
}
