using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace EmpManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {
           
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating ( ModelBuilder builder )
        {
            base.OnModelCreating ( builder );
    
            foreach (var foreignKey in builder.Model.GetEntityTypes ( ) 
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
