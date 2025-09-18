using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyTeProject.BackEnd.Entities.User;

namespace MyTeProject.BackEnd.Entities.WBSEntities
{
    public class UserWBS
    {
        [Key]
        [Column("User_WBS_Id")]
        public int Id { get; set; }

        [Required]
        [Column("User_WBS_WBS_Id")]
        public AppUser User { get; set; }

        [Required]
        [Column("User_WBS_User_Id")]
        public WBS WBS { get; set; }
    }
}
