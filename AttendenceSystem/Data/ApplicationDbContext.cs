using AttendenceSystem.Data.Models.Attendence;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        
        }
        public DbSet<LeaveModel> Leaves { get; set; }
        public DbSet<AttendenceHistoryModel> AttendenceHistory { get; set; }
        public DbSet<LeavePolicy> LeavePolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure LeaveModel relationship
            builder.Entity<LeaveModel>()
                .HasOne(l => l.ApprovedBy)
                .WithMany()
                .HasForeignKey(l => l.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Seed default user
            SeedDefaultUser(builder);
        }

        private void SeedDefaultUser(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();
            
            var defaultUser = new IdentityUser
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

            builder.Entity<IdentityUser>().HasData(defaultUser);
        }
    }
}
