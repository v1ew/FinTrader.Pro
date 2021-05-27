using FinTrader.Pro.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrader.Pro.DB.Data
{
    public class FinTraderDataContext : DbContext
    {
        public FinTraderDataContext(DbContextOptions<FinTraderDataContext> options)
            : base(options) { }

        public DbSet<Bond> Bonds { get; set; }
        
        public DbSet<Coupon> Coupons { get; set; }
        
        public DbSet<Config> Config { get; set; }
        
        public DbSet<TradeDate> TradeDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bond>()
                .HasKey(b => b.SecId);

            modelBuilder.Entity<Bond>()
                .HasIndex(b => b.Isin);

            modelBuilder.Entity<Coupon>()
                .HasIndex(c => c.Isin);

            modelBuilder.Entity<Config>().HasNoKey();
            
            // Значения параметров по умолчанию
            modelBuilder.Entity<Config>().Property(c => c.BondsCount).HasDefaultValue(6);
            modelBuilder.Entity<Config>().Property(c => c.MaxYield).HasDefaultValue(15);
        }
    }
}
