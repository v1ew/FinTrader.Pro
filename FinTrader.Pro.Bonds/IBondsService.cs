using System.Threading.Tasks;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds
{
    public interface IBondsService
    {
        Task<Bond[]> SelectBondsAsync();

        Task DiscardWrongBondsAsync();

        Task UpdateStorage();
    }
}