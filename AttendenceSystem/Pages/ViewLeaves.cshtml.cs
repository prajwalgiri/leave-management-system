using AttendenceSystem.Data;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class ViewLeavesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ViewLeavesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public List<LeaveModel> Leaves { get; set; } = new List<LeaveModel>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return;
            }
            var userId = user.Id;
            var query = _context.Leaves.Where(l => l.EmployeeId == userId);
            if (StartDate.HasValue)
            {
                query = query.Where(l => l.StartDate >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                query = query.Where(l => l.EndDate <= EndDate.Value);
            }
            Leaves = await query.OrderByDescending(l => l.StartDate).ToListAsync();
        }
    }
}