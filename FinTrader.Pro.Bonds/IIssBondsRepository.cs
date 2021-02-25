using FinTrader.Pro.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Bonds
{
    public interface IIssBondsRepository
    {
        Task<IEnumerable<Bond>> LoadAsync();
    }
}