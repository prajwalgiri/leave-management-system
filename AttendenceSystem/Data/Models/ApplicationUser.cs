using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity;

namespace AttendenceSystem.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public Int64? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Navigation property for leaves
        public ICollection<LeaveModel> Leaves { get; set; } = new List<LeaveModel>();
    }
}
