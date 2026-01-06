using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    [Authorize(Roles = "Admin,Supervisor")]
    public class ApproveLeaveModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApproveLeaveModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<LeaveRequestViewModel> PendingLeaves { get; set; } = new();

        public class LeaveRequestViewModel
        {
            public Int64 Id { get; set; }
            public string EmployeeName { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Reason { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            IQueryable<LeaveModel> query = _context.Leaves
                .Include(l => l.ApprovedBy)
                .Where(l => l.Status == LeaveStatus.Requested);

            if (!User.IsInRole("Admin"))
            {
                // Supervisor only see their department
                query = query.Join(_context.Users,
                    l => l.EmployeeId,
                    u => u.Id,
                    (l, u) => new { Leave = l, User = u })
                    .Where(x => x.User.DepartmentId == user.DepartmentId)
                    .Select(x => x.Leave);
            }

            var results = await query.ToListAsync();

            // Map to ViewModel (Manual mapping to include department name and user name)
            // In a real app, use projection in the query for performance
            foreach (var leave in results)
            {
                var emp = await _userManager.FindByIdAsync(leave.EmployeeId);
                var dept = emp?.DepartmentId != null ? await _context.Departments.FindAsync(emp.DepartmentId) : null;

                PendingLeaves.Add(new LeaveRequestViewModel
                {
                    Id = leave.Id,
                    EmployeeName = emp?.FullName ?? emp?.UserName ?? "Unknown",
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Reason = leave.Reason,
                    Department = dept?.Name ?? "N/A"
                });
            }
        }

        public async Task<IActionResult> OnPostApproveAsync(Int64 id)
        {
            await UpdateLeaveStatus(id, LeaveStatus.Approved);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(Int64 id)
        {
            await UpdateLeaveStatus(id, LeaveStatus.Rejected);
            return RedirectToPage();
        }

        private async Task UpdateLeaveStatus(Int64 id, LeaveStatus status)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                leave.Status = status;
                leave.ApprovedAt = DateTime.Now;
                leave.ApprovedById = currentUser?.Id;
                leave.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
    }
}
