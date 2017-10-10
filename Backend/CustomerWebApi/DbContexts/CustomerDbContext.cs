using CustomerWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi.DbContexts
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
