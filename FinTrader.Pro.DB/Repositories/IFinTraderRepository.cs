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
        
        IQueryable<BondChange> BondChanges { get; }
        
        IQueryable<MarketRecord> MarketRecords { get; }

        Task AddBondsRangeAsync(Bond[] bonds);

        Task AddCouponsRangeAsync(Coupon[] coupons);

        Task AddMarketRecordsRangeAsync(MarketRecord[] records);

        Task UpdateBondsRangeAsync(IEnumerable<Bond> bonds);

        Task UpdateCouponsRangeAsync(IEnumerable<Coupon> coupons);
        
        Task UpdateMarketRecordsRangeAsync(IEnumerable<MarketRecord> records);

        Task ClearCacheAsync();
    }
}
