using System.Globalization;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages.Dashboards
{
    public class MonthlyLeaveScheduleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MonthlyLeaveScheduleModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public List<int> SelectedMonths { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int SelectedYear { get; set; } = DateTime.Today.Year;

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedUserIds { get; set; } = new();

        public List<SelectListItem> MonthOptions { get; set; } = new();
        public List<SelectListItem> UserOptions { get; set; } = new();
        public List<MonthGroupViewModel> Schedule { get; set; } = new();

        public class MonthGroupViewModel
        {
            public string MonthName { get; set; } = string.Empty;
            public int MonthValue { get; set; }
            public List<DayLeaveViewModel> Days { get; set; } = new();
        }

        public class DayLeaveViewModel
        {
            public DateTime Date { get; set; }
            public List<string> EmployeeNames { get; set; } = new();
        }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return;

            // Initialize options
            for (int i = 1; i <= 12; i++)
            {
                MonthOptions.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                    Selected = SelectedMonths.Contains(i)
                });
            }

            // Default to current month if none selected
            if (!SelectedMonths.Any())
            {
                SelectedMonths.Add(DateTime.Today.Month);
                MonthOptions[DateTime.Today.Month - 1].Selected = true;
            }

            IQueryable<ApplicationUser> usersQuery;
            if (User.IsInRole("Admin"))
            {
                usersQuery = _context.Users;
            }
            else
            {
                usersQuery = _context.Users.Where(u => u.DepartmentId == currentUser.DepartmentId);
            }

            var availableUsers = await usersQuery.Select(u => new { u.Id, Name = u.FullName ?? u.UserName }).ToListAsync();
            UserOptions = availableUsers.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.Name,
                Selected = SelectedUserIds.Contains(u.Id)
            }).ToList();

            // Fetch approved leaves
            var leavesQuery = _context.Leaves
                .Include(l => l.ApprovedBy)
                .Where(l => l.Status == LeaveStatus.Approved);

            // Filter by selected users if provided
            if (SelectedUserIds.Any())
            {
                leavesQuery = leavesQuery.Where(l => SelectedUserIds.Contains(l.EmployeeId));
            }
            else if (!User.IsInRole("Admin"))
            {
                // If not Admin and no specific users selected, default to department
                var deptUserIds = availableUsers.Select(u => u.Id).ToList();
                leavesQuery = leavesQuery.Where(l => deptUserIds.Contains(l.EmployeeId));
            }

            var approvedLeaves = await leavesQuery.ToListAsync();
            var userMap = availableUsers.ToDictionary(u => u.Id, u => u.Name);

            foreach (var month in SelectedMonths.OrderBy(m => m))
            {
                var monthGroup = new MonthGroupViewModel
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    MonthValue = month
                };

                var daysInMonth = DateTime.DaysInMonth(SelectedYear, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(SelectedYear, month, day);
                    var dayViewModel = new DayLeaveViewModel { Date = date };

                    foreach (var leave in approvedLeaves)
                    {
                        if (date >= leave.StartDate.Date && date <= leave.EndDate.Date)
                        {
                            if (userMap.TryGetValue(leave.EmployeeId, out var name))
                            {
                                if (name != null) dayViewModel.EmployeeNames.Add(name);
                            }
                        }
                    }

                    if (dayViewModel.EmployeeNames.Any())
                    {
                        monthGroup.Days.Add(dayViewModel);
                    }
                }

                Schedule.Add(monthGroup);
            }
        }
    }
}
