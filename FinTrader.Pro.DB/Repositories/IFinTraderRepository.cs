using System;
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
        
        IQueryable<Config> Config { get; }

        IQueryable<TradeDate> TradeDates { get; }


        Task AddBondsRangeAsync(Bond[] bonds);

        Task AddCouponsRangeAsync(Coupon[] coupons);

        Task AddTradeDatesAsync(TradeDate[] dates);

        Task UpdateBondsRangeAsync(IEnumerable<Bond> bonds);

        Task UpdateCouponsRangeAsync(IEnumerable<Coupon> coupons);

        Task RemoveCouponsAsync(Coupon[] coupons);

        Task UpdateConfigAsync(Config config);
        
        Task ClearCacheAsync();
    }
}
