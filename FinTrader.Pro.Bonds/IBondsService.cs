using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.Contracts.Bonds;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds
{
    public interface IBondsService
    {
        Task<Portfolio[]> SelectBondsAsync(BondsPickerParams picker);

        Task DiscardWrongBondsAsync();

        Task CheckCouponsAsync();

        Task UpdateBondsAsync();

        Task UpdateBondsValueAsync();
        
        Task UpdateCouponsAsync();

        Task UpdateBondsDurationAsync();

        Task<DateTime> UpdateTradeDateAsync();
    }
}