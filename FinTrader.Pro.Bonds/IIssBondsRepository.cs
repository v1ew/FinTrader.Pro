using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Bonds
{
    public interface IIssBondsRepository
    {
        Task<IEnumerable<Dictionary<string, string>>> LoadBondsAsync();

        Task<IEnumerable<Dictionary<string, string>>> LoadBondsMarketDataAsync();

        Task<IEnumerable<Dictionary<string, string>>> LoadCouponsAsync(string secId);
    }
}