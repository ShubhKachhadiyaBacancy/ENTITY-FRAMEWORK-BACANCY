using DAY1.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY1.DataContext
{
    public class EFCoreDataContext :DbContext
    {

        public EFCoreDataContext(DbContextOptions<EFCoreDataContext> options) : base(options) 
        {

        }
        public DbSet<Student> StudentsUsingEnvironmentString { get; set; }    
    }
}
