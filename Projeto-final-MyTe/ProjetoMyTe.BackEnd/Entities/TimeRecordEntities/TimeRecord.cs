using MyTeProject.BackEnd.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyTeProject.BackEnd.Entities.WBSEntities;
using System.Diagnostics.CodeAnalysis;

namespace MyTeProject.BackEnd.Entities.TimeRecordEntities
{
    public class TimeRecord
    {
        [Key]
        [Column("TimeRecord_Id")]
        public int Id { get; set; }

        [Required]
        [Column("TimeRecord_Date")]
        public DateOnly Date { get; set; }

        [Column("TimeRecord_AppointedTime")]
        [AllowNull]
        public double? AppointedTime { get; set; }

        [Column("TimeRecord_WBS")]
        [AllowNull]
        public WBS? WBS { get; set; }

        [Required]
        [Column("TimeRecord_Fortnight")]
        public Fortnight Fortnight { get; set; }
    }
}
