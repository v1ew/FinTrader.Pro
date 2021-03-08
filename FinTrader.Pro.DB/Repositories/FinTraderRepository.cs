using FinTrader.Pro.DB.Data;
using FinTrader.Pro.DB.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FinTrader.Pro.DB.Repositories
{
    public class FinTraderRepository : IFinTraderRepository
    {
        private FinTraderDataContext context;

        public FinTraderRepository(FinTraderDataContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Bond> Bonds => context.Bonds;

        public async Task AddBondsRangeAsync(Bond[] bonds)
        {
            await context.Bonds.AddRangeAsync(bonds);
            await context.SaveChangesAsync(default);
        }

        public async Task AddCouponsRangeAsync(Coupon[] coupons)
        {
            await context.Coupons.AddRangeAsync(coupons);
            await context.SaveChangesAsync(default);
        }

        public async Task ClearCacheAsync()
        {
            context.Bonds.RemoveRange(Bonds);
            await context.SaveChangesAsync(default);
        }
    }
}
