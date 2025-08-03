namespace AttendenceSystem.Data.Models
{
    public class BaseModel
    {
        public Int64 Id { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
