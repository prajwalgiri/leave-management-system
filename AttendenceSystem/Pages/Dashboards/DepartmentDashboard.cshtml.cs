using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages.Dashboards
{
    public class DepartmentDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepartmentDashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string DepartmentName { get; set; } = "N/A";
        public List<LeaveViewModel> DepartmentLeaves { get; set; } = new();

        public class LeaveViewModel
        {
            public string EmployeeName { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public LeaveStatus Status { get; set; }
            public string Reason { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.DepartmentId == null) return;

            var dept = await _context.Departments.FindAsync(user.DepartmentId);
            DepartmentName = dept?.Name ?? "N/A";

            var employeesInDept = await _userManager.Users
                .Where(u => u.DepartmentId == user.DepartmentId)
                .Select(u => new { u.Id, FullName = u.FullName ?? u.UserName })
                .ToListAsync();

            var employeeIds = employeesInDept.Select(e => e.Id).ToList();

            var leaves = await _context.Leaves
                .Where(l => employeeIds.Contains(l.EmployeeId))
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            foreach (var leave in leaves)
            {
                var emp = employeesInDept.First(e => e.Id == leave.EmployeeId);
                DepartmentLeaves.Add(new LeaveViewModel
                {
                    EmployeeName = emp.FullName ?? "Unknown",
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Status = leave.Status,
                    Reason = leave.Reason
                });
            }
        }
    }
}
