using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages.Dashboards
{
    public class TodaysLeavesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodaysLeavesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<OnLeaveViewModel> EmployeesOnLeave { get; set; } = new();

        public class OnLeaveViewModel
        {
            public string Name { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public DateTime EndDate { get; set; }
        }

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;

            var activeLeaves = await _context.Leaves
                .Include(l => l.ApprovedBy)
                .Where(l => l.Status == LeaveStatus.Approved)
                .Where(l => l.StartDate <= today && l.EndDate >= today)
                .ToListAsync();

            var userIds = activeLeaves.Select(l => l.EmployeeId).Distinct().ToList();
            var users = await _context.Users
                .Include(u => u.Department)
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            foreach (var leave in activeLeaves)
            {
                var user = users.FirstOrDefault(u => u.Id == leave.EmployeeId);
                if (user != null)
                {
                    EmployeesOnLeave.Add(new OnLeaveViewModel
                    {
                        Name = user.FullName ?? user.UserName ?? "Unknown",
                        Department = user.Department?.Name ?? "N/A",
                        EndDate = leave.EndDate
                    });
                }
            }
        }
    }
}
