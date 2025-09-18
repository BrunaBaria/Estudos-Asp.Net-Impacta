using MyTeProject.BackEnd.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeProject.BackEnd.Entities.Admin
{
    public class Department
    {
        [Key]
        [Column("Department_Id")]
        public int Id { get; set; }

        [Required]
        [Column("Department_Name")]
        public string? Name { get; set; }

        [Required]
        [Column("Department_Contact_Email")]
        public string? ContactEmail { get; set; }

        public IEnumerable<AppUser>? Users { get; set; }
    }
}
