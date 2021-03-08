﻿using FinTrader.Pro.DB.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FinTrader.Pro.DB.Repositories
{
    public interface IFinTraderRepository
    {
        IQueryable<Bond> Bonds { get; }

        Task AddBondsRangeAsync(Bond[] bonds);

        Task AddCouponsRangeAsync(Coupon[] coupons);

        Task ClearCacheAsync();
    }
}
