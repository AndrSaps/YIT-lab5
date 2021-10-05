using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;

namespace Milkify.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            //membership
            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserRole>();

            //buisness logic

            modelBuilder.Entity<Order>()
                .HasOne<Shipment>(s => s.Shipment)
                .WithOne(ad => ad.Order)
                .HasForeignKey<Shipment>(ad => ad.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLineItem>();

            modelBuilder.Entity<AudioRecord>();

            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<Factory>();

            modelBuilder.Entity<Location>();

            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Category>();

            modelBuilder.Entity<Inventory>();

            modelBuilder.Entity<Truck>();

            modelBuilder.Entity<Payment>();
            modelBuilder.Entity<Shipment>();

            modelBuilder.Entity<Route>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
