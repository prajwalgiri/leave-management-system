using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Attendence;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<LeaveModel> Leaves { get; set; }
        public DbSet<AttendenceHistoryModel> AttendenceHistory { get; set; }
        public DbSet<LeavePolicy> LeavePolicies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure LeaveModel relationship
            builder.Entity<LeaveModel>()
                .HasOne(l => l.ApprovedBy)
                .WithMany()
                .HasForeignKey(l => l.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure ApplicationUser to Department relationship
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Department Head relationship
            builder.Entity<Department>()
                .HasOne(d => d.DepartmentHead)
                .WithMany()
                .HasForeignKey(d => d.DepartmentHeadId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed default values (moved to DataSeeder for complexity)
            // SeedDefaultUser(builder); 
        }

        // Seeding moved to DataSeeder.cs for more flexibility with Roles and Departments
        /*
        private void SeedDefaultUser(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            
            var defaultUser = new ApplicationUser
            {
                Id = "1",
                UserName = "admin@company.com",
                NormalizedUserName = "ADMIN@COMPANY.COM",
                Email = "admin@company.com",
                NormalizedEmail = "ADMIN@COMPANY.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            defaultUser.PasswordHash = hasher.HashPassword(defaultUser, "Admin123!");

            builder.Entity<ApplicationUser>().HasData(defaultUser);
        }
        */
    }
}
