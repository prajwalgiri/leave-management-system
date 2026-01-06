using System.ComponentModel.DataAnnotations;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class AddEmployeeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AddEmployeeModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<Department> Departments { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string FullName { get; set; } = string.Empty;

            public string? PhoneNumber { get; set; }

            [Required]
            public Int64 DepartmentId { get; set; }

            [Required]
            public string Role { get; set; } = "User";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            Departments = await _context.Departments.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Departments = await _context.Departments.ToListAsync();
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.UserName,
                Email = Input.Email,
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber,
                DepartmentId = Input.DepartmentId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Input.Role);
                return RedirectToPage("/Employees");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Departments = await _context.Departments.ToListAsync();
            return Page();
        }
    }
}