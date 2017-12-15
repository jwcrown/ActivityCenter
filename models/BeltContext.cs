using Microsoft.EntityFrameworkCore;
 
namespace BeltExam1.Models
{
    public class BeltContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Signup> Signups { get; set; }
    }
} 