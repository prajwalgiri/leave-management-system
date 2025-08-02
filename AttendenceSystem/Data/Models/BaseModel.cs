namespace AttendenceSystem.Data.Models
{
    public class BaseModel(DateTime CreatedAt = default(DateTime), DateTime UpdatedAt = default(DateTime))
    {
        public Int64 Id { get; set; } 
        public DateTime CreatedAt { get; set; } = CreatedAt == default ? DateTime.Now : CreatedAt;
        public DateTime UpdatedAt { get; set; } = UpdatedAt == default ? DateTime.Now : UpdatedAt;
    }

}
