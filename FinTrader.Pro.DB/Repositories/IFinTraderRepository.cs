using FinTrader.Pro.DB.Models;
using System.Linq;

namespace FinTrader.Pro.DB.Repositories
{
    public interface IFinTraderRepository
    {
        IQueryable<Bond> Bonds { get; }
    }
}
