using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyTeProject.BackEnd.Entities.User;

namespace MyTeProject.BackEnd.Entities.Admin
{
    public class Location
    {
        [Key]
        [Column("Location_Id")]
        public int Id { get; set; }

        [Required]
        [Column("Location_State")]
        [MaxLength(2)]
        public string? State { get; set; }

        [Required]
        [Column("Location_City")]
        public string? City { get; set; }

        public IEnumerable<AppUser>? Users { get; set; }
    }
}
