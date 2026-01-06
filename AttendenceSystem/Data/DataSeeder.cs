using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Attendence;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Supervisor", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            // Check if departments exist, if not create some
            if (!await context.Departments.AnyAsync())
            {
                var depts = new List<Department>
                {
                    new Department { Name = "IT", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new Department { Name = "HR", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new Department { Name = "Finance", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                };
                context.Departments.AddRange(depts);
                await context.SaveChangesAsync();
            }

            var itDept = await context.Departments.FirstAsync(d => d.Name == "IT");
            var hrDept = await context.Departments.FirstAsync(d => d.Name == "HR");

            // Check if users already exist
            if (await userManager.Users.AnyAsync())
            {
                // Ensure existing users have departments if they don't
                var existingUsers = await userManager.Users.Where(u => u.DepartmentId == null).ToListAsync();
                foreach (var u in existingUsers)
                {
                    u.DepartmentId = itDept.Id;
                    await userManager.UpdateAsync(u);
                }
                return;
            }

            var users = new List<(ApplicationUser User, string Password, string Role)>
            {
                (new ApplicationUser
                {
                    UserName = "admin@company.com",
                    Email = "admin@company.com",
                    FullName = "System Admin",
                    EmailConfirmed = true,
                    DepartmentId = itDept.Id
                }, "Admin123!", "Admin"),

                (new ApplicationUser
                {
                    UserName = "supervisor@company.com",
                    Email = "supervisor@company.com",
                    FullName = "IT Supervisor",
                    EmailConfirmed = true,
                    DepartmentId = itDept.Id
                }, "Password123!", "Supervisor"),

                (new ApplicationUser
                {
                    UserName = "employee1@company.com",
                    Email = "employee1@company.com",
                    FullName = "John Doe",
                    EmailConfirmed = true,
                    DepartmentId = itDept.Id
                }, "Password123!", "User"),

                (new ApplicationUser
                {
                    UserName = "hr_manager@company.com",
                    Email = "hr_manager@company.com",
                    FullName = "HR Manager",
                    EmailConfirmed = true,
                    DepartmentId = hrDept.Id
                }, "Password123!", "Supervisor")
            };

            foreach (var item in users)
            {
                var result = await userManager.CreateAsync(item.User, item.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(item.User, item.Role);

                    // If they are supervisor, maybe make them head of department (simple logic for seeding)
                    if (item.Role == "Supervisor" && item.User.UserName == "supervisor@company.com")
                    {
                        itDept.DepartmentHeadId = item.User.Id;
                        context.Departments.Update(itDept);
                    }
                    else if (item.Role == "Supervisor" && item.User.UserName == "hr_manager@company.com")
                    {
                        hrDept.DepartmentHeadId = item.User.Id;
                        context.Departments.Update(hrDept);
                    }
                }
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "admin@company.com");
            var employeeUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "employee1@company.com");

            if (adminUser == null || employeeUser == null) return;

            // Add sample leave requests
            if (!context.Leaves.Any())
            {
                var sampleLeaves = new List<LeaveModel>
                {
                    new LeaveModel
                    {
                        EmployeeId = employeeUser.Id,
                        StartDate = DateTime.Now.AddDays(7),
                        EndDate = DateTime.Now.AddDays(10),
                        Reason = "Annual vacation",
                        Status = LeaveStatus.Requested,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new LeaveModel
                    {
                        EmployeeId = employeeUser.Id,
                        StartDate = DateTime.Now.AddDays(14),
                        EndDate = DateTime.Now.AddDays(16),
                        Reason = "Medical appointment",
                        Status = LeaveStatus.Approved,
                        ApprovedAt = DateTime.Now,
                        ApprovedById = adminUser.Id,
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
                        EmployeeId = Guid.Parse(employeeUser.Id),
                        Date = DateOnly.FromDateTime(DateTime.Today),
                        Time = TimeOnly.FromDateTime(DateTime.Now),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };

                context.AttendenceHistory.AddRange(sampleAttendance);
                await context.SaveChangesAsync();
            }
        }
    }
}
