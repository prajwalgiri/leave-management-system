using AttendenceSystem.Data;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class RequestLeaveModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RequestLeaveModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public LeaveInputModel Leave { get; set; } = new LeaveInputModel();

        public int TotalLeaves { get; set; }

        public class LeaveInputModel
        {
            [Required]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [Required]
            public string Reason { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                TotalLeaves = await _context.Leaves.CountAsync(l => l.EmployeeId == user.Id);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    TotalLeaves = await _context.Leaves.CountAsync(l => l.EmployeeId == user.Id);
                }
                return Page();
            }

            var userPost = await _userManager.GetUserAsync(User);
            if (userPost == null)
            {
                return Challenge();
            }

            var leave = new LeaveModel
            {
                EmployeeId = userPost.Id,
                StartDate = Leave.StartDate,
                EndDate = Leave.EndDate,
                Reason = Leave.Reason,
                Status = LeaveStatus.Requested
            };

            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}