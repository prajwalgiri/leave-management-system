using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AttendenceSystem.Data.Models.Leave;
using AttendenceSystem.Data.Models.Attendence;

namespace AttendenceSystem.Data
{
    public static class DataSeeder
    {
        public static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            // Check if users already exist
            if (await userManager.Users.AnyAsync())
                return;

            var users = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "admin@company.com",
                    Email = "admin@company.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    UserName = "employee1@company.com",
                    Email = "employee1@company.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    UserName = "manager@company.com",
                    Email = "manager@company.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                }
            };

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "Password123!");
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user {user.UserName}: {errors}");
                }
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Add sample leave requests
            if (!context.Leaves.Any())
            {
                var sampleLeaves = new List<LeaveModel>
                {
                    new LeaveModel
                    {
                        EmployeeId = "sample-user-id-1",
                        StartDate = DateTime.Now.AddDays(7),
                        EndDate = DateTime.Now.AddDays(10),
                        Reason = "Annual vacation",
                        Status = LeaveStatus.Requested,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new LeaveModel
                    {
                        EmployeeId = "sample-user-id-2",
                        StartDate = DateTime.Now.AddDays(14),
                        EndDate = DateTime.Now.AddDays(16),
                        Reason = "Medical appointment",
                        Status = LeaveStatus.Approved,
                        ApprovedAt = DateTime.Now,
                        CreatedAt = DateTime.Now.AddDays(-5),
                        UpdatedAt = DateTime.Now
                    }
                };

                context.Leaves.AddRange(sampleLeaves);
                await context.SaveChangesAsync();
            }

            // Add sample attendance records
            if (!context.AttendenceHistory.Any())
            {
                var sampleAttendance = new List<AttendenceHistoryModel>
                {
                    new AttendenceHistoryModel
                    {
                        EmployeeId = Guid.NewGuid(),
                        Date = DateOnly.FromDateTime(DateTime.Today),
                        Time = TimeOnly.FromDateTime(DateTime.Now),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new AttendenceHistoryModel
                    {
                        EmployeeId = Guid.NewGuid(),
                        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                        Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)),
                        CreatedAt = DateTime.Now.AddDays(-1),
                        UpdatedAt = DateTime.Now.AddDays(-1)
                    }
                };

                context.AttendenceHistory.AddRange(sampleAttendance);
                await context.SaveChangesAsync();
            }
        }
    }
} 