using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AttendenceSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<IndexModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public int AllowedLeaves { get; set; }
        public List<EmployeeLeaveViewModel> Employees { get; set; } = new();

        public class EmployeeLeaveViewModel
        {
            public string Name { get; set; } = string.Empty;
            public int TotalLeaves { get; set; }
            public bool Exceeded { get; set; }
        }

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            var policy = await _context.LeavePolicies.OrderByDescending(p => p.ToDate).FirstOrDefaultAsync();
            AllowedLeaves = policy?.AllowedLeaves ?? 0;
            var from = FromDate ?? policy?.FromDate ?? new DateTime(DateTime.Now.Year, 1, 1);
            var to = ToDate ?? policy?.ToDate ?? new DateTime(DateTime.Now.Year, 12, 31);
            var leaves = await _context.Leaves
                .Where(l => l.StartDate >= from && l.EndDate <= to)
                .ToListAsync();
            Employees = users.Select(u => new EmployeeLeaveViewModel
            {
                Name = u.UserName ?? u.Email ?? "(No Name)",
                TotalLeaves = leaves.Count(l => l.EmployeeId == u.Id),
                Exceeded = AllowedLeaves > 0 && leaves.Count(l => l.EmployeeId == u.Id) > AllowedLeaves
            }).ToList();
        }
    }
}
