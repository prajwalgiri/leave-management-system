using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceSystem.Data.Models
{
    public class Department : BaseModel
    {
        public required string Name { get; set; }

        public string? DepartmentHeadId { get; set; }

        [ForeignKey("DepartmentHeadId")]
        public ApplicationUser? DepartmentHead { get; set; }

        public ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
    }
}
