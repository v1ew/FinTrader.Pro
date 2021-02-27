using FinTrader.Pro.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.DB.Data
{
    public class FinTraderDataContext : DbContext
    {
        public FinTraderDataContext(DbContextOptions<FinTraderDataContext> options)
            : base(options) { }

        public DbSet<Bond> Bonds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bond>().HasKey(b => new { b.SecId, b.BoardId });
        }
    }
}
