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
        public IQueryable<BondChange> BondChanges => _context.BondChanges;

        public async Task UpdateBondsRangeAsync(IEnumerable<Bond> bonds)
        {
            _context.Bonds.UpdateRange(bonds);
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

        public async Task ClearCacheAsync()
        {
            _context.Bonds.RemoveRange(Bonds);
            await _context.SaveChangesAsync(default);
        }
    }
}
