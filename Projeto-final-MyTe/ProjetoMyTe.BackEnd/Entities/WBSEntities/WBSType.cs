using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyTeProject.BackEnd.Entities.WBSEntities
{
    public class WBSType
    {
        [Key]
        [Column("WBS_Type_Id")]
        public int Id { get; set; }

        [Required]
        [Column("WBS_Type_Description")]
        public string? Description { get; set; }

        public IEnumerable<WBS>? WBSList { get; set; }
    }
}
