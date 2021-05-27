using System;
using FinTrader.Pro.DB.Data;
using FinTrader.Pro.DB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinTrader.Pro.DB.Repositories
{
    public class FinTraderRepository : IFinTraderRepository
    {
        private readonly FinTraderDataContext _context;

        public FinTraderRepository(FinTraderDataContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Bond> Bonds => _context.Bonds;
        public IQueryable<Coupon> Coupons => _context.Coupons;
        public IQueryable<TradeDate> TradeDates => _context.TradeDates;
        public IQueryable<Config> Config => _context.Config;

        public async Task UpdateBondsRangeAsync(IEnumerable<Bond> bonds)
        {
            _context.Bonds.UpdateRange(bonds);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCouponsRangeAsync(IEnumerable<Coupon> coupons)
        {
            _context.Coupons.UpdateRange(coupons);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConfigAsync(Config config)
        {
            _context.Config.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task AddBondsRangeAsync(Bond[] bonds)
        {
            await _context.Bonds.AddRangeAsync(bonds);
            await _context.SaveChangesAsync(default);
        }

        public async Task AddCouponsRangeAsync(Coupon[] coupons)
        {
            await _context.Coupons.AddRangeAsync(coupons);
            await _context.SaveChangesAsync(default);
        }

        public async Task AddTradeDateAsync(DateTime date)
        {
            await _context.TradeDates.AddAsync(new TradeDate { Date = date });
            await _context.SaveChangesAsync(default);
        }
        
        public async Task ClearCacheAsync()
        {
            _context.Bonds.RemoveRange(Bonds);
            await _context.SaveChangesAsync(default);
        }
    }
}
