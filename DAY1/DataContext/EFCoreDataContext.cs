using DAY1.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY1.DataContext
{
    public class EFCoreDataContext :DbContext
    {
        public DbSet<Student> Students { get; set; }
    }
}
