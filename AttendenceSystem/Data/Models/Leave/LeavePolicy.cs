using AttendenceSystem.Data.Models;

namespace AttendenceSystem.Data.Models.Leave
{
    public class LeavePolicy : BaseModel
    {
        public int AllowedLeaves { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}