using AttendenceSystem.Data.Models.Attendence;
using AttendenceSystem.Data.Models.Leave;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<LeaveModel> Leaves { get; set; }
        public DbSet<AttendenceHistoryModel> AttendenceHistory { get; set; }

        
    }
}
