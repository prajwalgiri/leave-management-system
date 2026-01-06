using System.ComponentModel.DataAnnotations;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    public class ManageDepartmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageDepartmentsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Department> Departments { get; set; } = new();
        public List<ApplicationUser> Supervisors { get; set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Name { get; set; } = string.Empty;
            public string? DepartmentHeadId { get; set; }
        }

        public async Task OnGetAsync()
        {
            Departments = await _context.Departments
                .Include(d => d.DepartmentHead)
                .ToListAsync();

            Supervisors = (await _userManager.GetUsersInRoleAsync("Supervisor")).ToList();
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            Supervisors.AddRange(admins); // Admins can also be heads
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var department = new Department
            {
                Name = Input.Name,
                DepartmentHeadId = Input.DepartmentHeadId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
