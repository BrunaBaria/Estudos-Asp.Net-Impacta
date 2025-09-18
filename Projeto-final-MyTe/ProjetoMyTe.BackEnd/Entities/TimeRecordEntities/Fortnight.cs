using MyTeProject.BackEnd.Entities.WBSEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyTeProject.BackEnd.Entities.User;

namespace MyTeProject.BackEnd.Entities.TimeRecordEntities 
{
    public class Fortnight
    {
        [Key]
        [Column("Fortnight_Id")]
        public int Id { get; set; }

        [Required]
        [Column("Fortnight_Work_Schedule", TypeName = "decimal(4,2)")]
        public decimal WorkSchedule { get; set; }

        [Required]
        [Column("Fortnight_AppUser")]
        public AppUser AppUser { get; set; }

       public IList<TimeRecord>? TimeRecords { get; set; } = [];
    }
}
