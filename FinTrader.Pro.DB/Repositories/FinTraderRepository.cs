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

        public async Task AddRangeAsync(Bond[] bonds)
        {
            await context.Bonds.AddRangeAsync(bonds);
            await context.SaveChangesAsync(default);
        }

        public async Task ClearCacheAsync()
        {
            context.Bonds.RemoveRange(Bonds);
            await context.SaveChangesAsync(default);
        }
    }
}
