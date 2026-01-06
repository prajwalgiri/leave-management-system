using System.ComponentModel.DataAnnotations;

namespace AttendenceSystem.Data.Models
{
    public class CompanyInfo : BaseModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string ContactNumber { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public string? LogoUrl { get; set; }
    }
}
