using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.BackEnd.Entities.TimeRecordEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeProject.BackEnd.Entities.WBSEntities
{
    public class WBS
    {
        [Key]
        [Column("WBS_Id")]
        public int Id { get; set; }

        [Required]
        [Column("WBS_Charge_Code", TypeName = "varchar(10)")]
        public string ChargeCode { get; set; }

        [Required]
        [Column("WBS_Description")]
        public string Description { get; set; }

        [Required]
        [Column("WBS_WBS_Type")]
        public WBSType WBSType { get; set; }

        public IEnumerable<TimeRecord>? TimeRecords { get; set; }

        public IEnumerable<UserWBS>? UsersWBS { get; set; }

        public IEnumerable<Expense>? Expenses { get; set; }
    }
}
