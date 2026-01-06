using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages.Dashboards
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int PendingLeaves { get; set; }
        public int TotalLeavesToday { get; set; }
        public int TotalLeaves { get; set; }
        public List<DepartmentSummary> DepartmentSummaries { get; set; } = new();

        public class DepartmentSummary
        {
            public string Name { get; set; } = string.Empty;
            public int EmployeeCount { get; set; }
            public int LeaveCount { get; set; }
        }

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;
            TotalEmployees = await _context.Users.CountAsync();
            TotalDepartments = await _context.Departments.CountAsync();
            PendingLeaves = await _context.Leaves.CountAsync(l => l.Status == AttendenceSystem.Data.Models.Leave.LeaveStatus.Requested);
            TotalLeavesToday = await _context.Leaves
                .CountAsync(l => l.Status == AttendenceSystem.Data.Models.Leave.LeaveStatus.Approved
                            && l.StartDate <= today && l.EndDate >= today);
            TotalLeaves = await _context.Leaves.CountAsync();

            DepartmentSummaries = await _context.Departments
                .Select(d => new DepartmentSummary
                {
                    Name = d.Name,
                    EmployeeCount = _context.Users.Count(u => u.DepartmentId == d.Id),
                    LeaveCount = _context.Leaves.Count(l => _context.Users.Any(u => u.Id == l.EmployeeId && u.DepartmentId == d.Id))
                })
                .ToListAsync();
        }
    }
}
