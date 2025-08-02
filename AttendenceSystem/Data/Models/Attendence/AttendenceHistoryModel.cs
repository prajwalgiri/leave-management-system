namespace AttendenceSystem.Data.Models.Attendence
{
    public class AttendenceHistoryModel(Guid EmployeeId, DateTime Date) : BaseModel()
    {
        public Guid EmployeeId { get; set; } = EmployeeId;
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(Date);
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(Date);
    }

}
