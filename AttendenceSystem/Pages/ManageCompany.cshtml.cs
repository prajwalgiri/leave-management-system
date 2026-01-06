using System.ComponentModel.DataAnnotations;
using AttendenceSystem.Data;
using AttendenceSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageCompanyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageCompanyModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public long Id { get; set; }

            [Required]
            [Display(Name = "Company Name")]
            public string Name { get; set; } = string.Empty;

            [Display(Name = "Office Address")]
            public string Address { get; set; } = string.Empty;

            [Display(Name = "Contact Number")]
            public string ContactNumber { get; set; } = string.Empty;

            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Logo URL")]
            public string? LogoUrl { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var company = await _context.CompanyInfo.FirstOrDefaultAsync();
            if (company != null)
            {
                Input = new InputModel
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    ContactNumber = company.ContactNumber,
                    Email = company.Email,
                    LogoUrl = company.LogoUrl
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var company = await _context.CompanyInfo.FirstOrDefaultAsync();
            if (company == null)
            {
                company = new CompanyInfo { CreatedAt = DateTime.Now };
                _context.CompanyInfo.Add(company);
            }

            company.Name = Input.Name;
            company.Address = Input.Address;
            company.ContactNumber = Input.ContactNumber;
            company.Email = Input.Email;
            company.LogoUrl = Input.LogoUrl;
            company.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Company information updated successfully.";
            return RedirectToPage();
        }
    }
}
