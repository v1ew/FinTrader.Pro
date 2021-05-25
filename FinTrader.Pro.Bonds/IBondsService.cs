using System.Threading.Tasks;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds
{
    public interface IBondsService
    {
        Task<BondSet> SelectBondsAsync(BondsPickerParams picker);

        Task DiscardWrongBondsAsync();

        Task CheckCoupons();

        Task UpdateBondsAsync();
        
        Task UpdateCouponsAsync();

        Task UpdateMarketDataAsync();
    }
}