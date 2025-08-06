using AttendenceSystem.Data;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class ManageLeavePolicyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ManageLeavePolicyModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            [Range(0, 365)]
            public int AllowedLeaves { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime FromDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime ToDate { get; set; }
        }

        public async Task OnGetAsync()
        {
            var policy = await _context.LeavePolicies.OrderByDescending(p => p.ToDate).FirstOrDefaultAsync();
            if (policy != null)
            {
                Input.AllowedLeaves = policy.AllowedLeaves;
                Input.FromDate = policy.FromDate;
                Input.ToDate = policy.ToDate;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var policy = await _context.LeavePolicies.OrderByDescending(p => p.ToDate).FirstOrDefaultAsync();
            if (policy == null)
            {
                policy = new LeavePolicy();
                _context.LeavePolicies.Add(policy);
            }
            policy.AllowedLeaves = Input.AllowedLeaves;
            policy.FromDate = Input.FromDate;
            policy.ToDate = Input.ToDate;
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}