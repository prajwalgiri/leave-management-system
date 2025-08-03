using Microsoft.AspNetCore.Identity;

namespace AttendenceSystem.Data.Models.Leave
{
    public class LeaveModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public required string Reason { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Requested;
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedById { get; set; }
        public IdentityUser? ApprovedBy { get; set; }
    }
}
