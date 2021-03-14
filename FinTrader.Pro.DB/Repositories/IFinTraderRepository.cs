using System.Collections.Generic;
using FinTrader.Pro.DB.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FinTrader.Pro.DB.Repositories
{
    public interface IFinTraderRepository
    {
        IQueryable<Bond> Bonds { get; }

        IQueryable<Coupon> Coupons { get; }

        Task AddBondsRangeAsync(Bond[] bonds);

        Task AddCouponsRangeAsync(Coupon[] coupons);

        Task UpdateBondsRangeAsync(IEnumerable<Bond> bonds);

        Task ClearCacheAsync();
    }
}
