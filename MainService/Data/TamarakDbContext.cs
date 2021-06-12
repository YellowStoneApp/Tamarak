using MainService.Data.DBModels;
using Microsoft.EntityFrameworkCore;

namespace MainService.Data
{
    public class TamarakDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerFollowers> CustomerFollowers { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<CustomerGift> CustomerGifts { get; set; }
        public DbSet<CustomerGiftPurchase> CustomerGiftPurchases { get; set; }

        public TamarakDbContext(DbContextOptions<TamarakDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasIndex(u => u.IdentityKey)
                .IsUnique();
            builder.Entity<CustomerGift>()
                .Property(cg => cg.Quantity)
                .HasDefaultValue(1);
            base.OnModelCreating(builder);
        }
    }
}