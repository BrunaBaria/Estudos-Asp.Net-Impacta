using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.BackEnd.Entities.TimeRecordEntities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;

namespace MyTeProject.BackEnd.Entities
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public DbSet<Department> Department { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<HiringRegime> HiringRegime { get; set; }
        public DbSet<WBSType> WBSType { get; set; }
        public DbSet<WBS> WBS { get; set; }
        public DbSet<UserWBS> UserWBS { get; set; }
        public DbSet<ExpenseType> ExpenseType { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<TimeRecord> TimeRecord { get; set; }
        public DbSet<Fortnight> Fortnight { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WBS>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.WBSType).WithMany(f => f.WBSList).OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<AppUser>(entity =>
            {
                entity.HasOne(e => e.HiringRegime).WithMany(f => f.Users).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Location).WithMany(f => f.Users).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Department).WithMany(f => f.Users).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserWBS>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User).WithMany(f => f.UsersWBS).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.WBS).WithMany(f => f.UsersWBS).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<TimeRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.WBS).WithMany(f => f.TimeRecords).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Fortnight>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.AppUser).WithMany(f => f.Fortnights).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.TimeRecords).WithOne(f => f.Fortnight).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User).WithMany(f => f.Expenses).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.WBS).WithMany(f => f.Expenses).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ExpenseType).WithMany(f => f.Expenses).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
