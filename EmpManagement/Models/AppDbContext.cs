using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace EmpManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {
           
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
