using AttendenceSystem.Data;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<EmployeeViewModel> Employees { get; set; } = new();

        public class EmployeeViewModel
        {
            public string Name { get; set; } = string.Empty;
            public int TotalLeaves { get; set; }
        }

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            var currentYear = DateTime.Now.Year;
            var leaves = await _context.Leaves
                .Where(l => l.StartDate.Year == currentYear)
                .ToListAsync();

            Employees = users.Select(u => new EmployeeViewModel
            {
                Name = u.UserName ?? u.Email ?? "(No Name)",
                TotalLeaves = leaves.Count(l => l.EmployeeId == u.Id)
            }).ToList();
        }
    }
}