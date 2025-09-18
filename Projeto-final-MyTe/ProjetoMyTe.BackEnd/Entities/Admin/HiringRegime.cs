using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyTeProject.BackEnd.Entities.WBSEntities;
using MyTeProject.BackEnd.Entities.User;

namespace MyTeProject.BackEnd.Entities.Admin
{
    public class HiringRegime
    {
        [Key]
        [Column("Hiring_Regime_Id")]
        public int Id { get; set; }

        [Required]
        [Column("Hiring_Regime_Description")]
        public string? Description { get; set; }

        [Required]
        [Column("Hiring_Regime_Accept_Overtime")]
        public bool AcceptOvertime { get; set; }

        [Required]
        [Column("Hiring_Regime_Work_Schedule", TypeName = "decimal(4,2)")]
        public decimal WorkSchedule { get; set; }

        public IEnumerable<AppUser>? Users { get; set; }
    }
}
