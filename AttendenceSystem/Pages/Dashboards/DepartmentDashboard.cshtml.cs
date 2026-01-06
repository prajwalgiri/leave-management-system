using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty(SupportsGet = true)]
        public long? DeptId { get; set; }

        public string DepartmentName { get; set; } = "All Departments";
        public List<Department> Departments { get; set; } = new();
        public List<LeaveViewModel> DepartmentLeaves { get; set; } = new();

        public class LeaveViewModel
        {
            public string EmployeeName { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public LeaveStatus Status { get; set; }
            public string Reason { get; set; } = string.Empty;
            public string ProcessedBy { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            IQueryable<LeaveModel> leavesQuery = _context.Leaves
                .Include(l => l.ApprovedBy);

            if (User.IsInRole("Admin"))
            {
                Departments = await _context.Departments.ToListAsync();
                if (DeptId.HasValue)
                {
                    var dept = await _context.Departments.FindAsync(DeptId.Value);
                    DepartmentName = dept?.Name ?? "N/A";

                    var employeeIds = await _context.Users
                        .Where(u => u.DepartmentId == DeptId.Value)
                        .Select(u => u.Id)
                        .ToListAsync();

                    leavesQuery = leavesQuery.Where(l => employeeIds.Contains(l.EmployeeId));
                }
                // If no DeptId, shows all leaves
            }
            else
            {
                if (user.DepartmentId == null) return;
                var dept = await _context.Departments.FindAsync(user.DepartmentId);
                DepartmentName = dept?.Name ?? "N/A";

                var employeeIds = await _context.Users
                    .Where(u => u.DepartmentId == user.DepartmentId)
                    .Select(u => u.Id)
                    .ToListAsync();

                leavesQuery = leavesQuery.Where(l => employeeIds.Contains(l.EmployeeId));
            }

            var leaves = await leavesQuery
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            var allUsers = await _context.Users.Include(u => u.Department).ToListAsync();

            foreach (var leave in leaves)
            {
                var emp = allUsers.FirstOrDefault(u => u.Id == leave.EmployeeId);
                DepartmentLeaves.Add(new LeaveViewModel
                {
                    EmployeeName = emp?.FullName ?? emp?.UserName ?? "Unknown",
                    Department = emp?.Department?.Name ?? "N/A",
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Status = leave.Status,
                    Reason = leave.Reason,
                    ProcessedBy = leave.ApprovedBy?.FullName ?? leave.ApprovedBy?.UserName ?? (leave.Status != LeaveStatus.Requested ? "System/Deleted User" : "")
                });
            }
        }
    }
}
