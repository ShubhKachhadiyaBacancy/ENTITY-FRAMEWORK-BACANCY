using DAY1.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY1.DataContext
{
    public class EFCoreDataContext : DbContext  
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-RQLUK45G\\SQLEXPRESS;Database=Student_Details;Trusted_Connection=True; TrustServerCertificate=True;");
            }
        }

        DbSet<Student> studentsUsingOnConfiguring { get; set; }
    }
}
