using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<EmployeeViewModel> EmployeeList { get; set; } = new();

        public class EmployeeViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? Department { get; set; }
            public int TotalLeaves { get; set; }
        }

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.Department)
                .ToListAsync();

            var currentYear = DateTime.Now.Year;
            var leaves = await _context.Leaves
                .Where(l => l.StartDate.Year == currentYear)
                .ToListAsync();

            EmployeeList = users.Select(u => new EmployeeViewModel
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Name = u.FullName ?? u.UserName ?? "(No Name)",
                Email = u.Email ?? string.Empty,
                Phone = u.PhoneNumber,
                Department = u.Department?.Name,
                TotalLeaves = leaves.Count(l => l.EmployeeId == u.Id)
            }).ToList();
        }
    }
}