using FinTrader.Pro.DB.Data;
using FinTrader.Pro.DB.Models;
using System.Linq;

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
    }
}
