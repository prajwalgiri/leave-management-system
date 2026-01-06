using System.Globalization;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages.Dashboards
{
    public class WeeklyLeavesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WeeklyLeavesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? SelectedDate { get; set; }

        public List<LeaveDayViewModel> WeeklyData { get; set; } = new();
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }

        public class LeaveDayViewModel
        {
            public DateTime Date { get; set; }
            public List<string> EmployeeNames { get; set; } = new();
        }

        public async Task OnGetAsync()
        {
            var today = SelectedDate ?? DateTime.Today;
            // Find start of week (Monday)
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            WeekStart = today.AddDays(-1 * diff).Date;
            WeekEnd = WeekStart.AddDays(6).Date;

            // Initialize the week
            for (int i = 0; i < 7; i++)
            {
                WeeklyData.Add(new LeaveDayViewModel { Date = WeekStart.AddDays(i) });
            }

            // Get all approved leaves that overlap with this week
            var approvedLeaves = await _context.Leaves
                .Where(l => l.Status == LeaveStatus.Approved)
                .Where(l => l.StartDate <= WeekEnd && l.EndDate >= WeekStart)
                .ToListAsync();

            // Fetch user info for mapping
            var userIds = approvedLeaves.Select(l => l.EmployeeId).Distinct().ToList();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName ?? u.UserName);

            foreach (var leave in approvedLeaves)
            {
                var userName = users.ContainsKey(leave.EmployeeId) ? users[leave.EmployeeId] : "Unknown";

                // For each day of the leave that falls within our week, add the user
                for (var date = WeekStart; date <= WeekEnd; date = date.AddDays(1))
                {
                    if (date >= leave.StartDate.Date && date <= leave.EndDate.Date)
                    {
                        var dayData = WeeklyData.First(d => d.Date == date);
                        if (userName != null) dayData.EmployeeNames.Add(userName);
                    }
                }
            }
        }
    }
}
