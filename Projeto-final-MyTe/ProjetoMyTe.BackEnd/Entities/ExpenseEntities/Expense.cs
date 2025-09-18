using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeProject.BackEnd.Entities.ExpenseEntities
{
    public class Expense
    {
        [Key]
        [Column("Expense_Id")]
        public int Id { get; set; }

        [Required]
        [Column("Expense_Date")]
        public DateOnly Date {  get; set; }

        [Required]
        [Column("Expense_Value")]
        public double Value { get; set; }

        [Required]
        [Column("Expense_Description")]
        public string? Description { get; set; }

        [Required]
        [Column("Expense_User")]
        public AppUser User { get; set; }

        [Required]
        [Column("Expense_WBS")]
        public WBS WBS { get; set; }

        [Required]
        [Column("Expense_Expense_Type")]
        public ExpenseType ExpenseType { get; set; }
    }
}
