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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bond>().HasKey(b => new { b.SecId, b.BoardId });
        }
    }
}
