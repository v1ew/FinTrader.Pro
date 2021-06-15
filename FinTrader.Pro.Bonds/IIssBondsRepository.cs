using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTrader.Pro.Iss.Requests;

namespace FinTrader.Pro.Bonds
{
    public interface IIssBondsRepository
    {
        Task<IEnumerable<Dictionary<string, string>>> LoadBondsAsync();

        Task<IEnumerable<Dictionary<string, string>>> LoadBondsDurationsAsync(DateTime date);
        
        Task<Dictionary<string, string>> LoadDatesAsync();

        Task<IEnumerable<Dictionary<string, string>>> LoadBondHistoryAsync(string secId, DateTime fromDate);
        
        Task<IEnumerable<Dictionary<string, string>>> LoadCouponsAsync(string secId);

        Task<IEnumerable<Dictionary<string, string>>> LoadAmortizationsAsync(string secId);

        Task<IEnumerable<Dictionary<string, string>>> LoadOffersAsync(string secId);
        
        Task<string> LoadAnyBondAsync();

        Task<IEnumerable<Dictionary<string, string>>> LoadBondHistoryDatesAsync(string secId, DateTime fromDate);

        Task<IEnumerable<Dictionary<string, string>>> LoadBondsInfoAsync(string secId);
    }
}