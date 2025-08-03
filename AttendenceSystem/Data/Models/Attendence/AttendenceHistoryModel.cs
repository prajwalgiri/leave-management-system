namespace AttendenceSystem.Data.Models.Attendence
{
    public class AttendenceHistoryModel : BaseModel
    {
        public Guid EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
    }
}
