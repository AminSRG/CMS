using CustomerManagementSystem.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Infrastructure.Persistence
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => new { c.FirstName, c.LastName, c.DateOfBirth })
                .IsUnique(true);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique(true);
        }
    }
}