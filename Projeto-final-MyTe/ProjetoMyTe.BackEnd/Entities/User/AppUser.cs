using Microsoft.AspNetCore.Identity;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.BackEnd.Entities.TimeRecordEntities;
using MyTeProject.BackEnd.Entities.WBSEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeProject.BackEnd.Entities.User
{
    public class AppUser : IdentityUser<int>
    {
        [Required]
        [Column("AppUser_Hiring_Regime")]
        public HiringRegime HiringRegime { get; set; }

        [Required]
        [Column("AppUser_Department")]
        public Department Department { get; set; }

        [Required]
        [Column("AppUser_Location")]
        public Location Location { get; set; }

        [Required]
        [Column("AppUser_Active")]
        public bool Active { get; set; }

        [Required]
        [Column("AppUser_Admission_Date")]
        public DateOnly AdmissionDate { get; set; }

        public IEnumerable<Fortnight>? Fortnights { get; set; }

        public IEnumerable<UserWBS>? UsersWBS { get; set; }

        public IEnumerable<Expense>? Expenses { get; set; }
    }
}
