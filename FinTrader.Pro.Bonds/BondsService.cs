using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.DB.Repositories;

namespace FinTrader.Pro.Bonds
{
    public class BondsService : IBondsService
    {
        private readonly IIssBondsRepository issBondsRepository;
        private readonly IFinTraderRepository traderRepository;
        public BondsService(IIssBondsRepository issBondsRepo, IFinTraderRepository traderRepo)
        {
            issBondsRepository = issBondsRepo;
            traderRepository = traderRepo;
        }

        public async Task UpdateStorage()
        {
            if (!traderRepository.Bonds.Any())
            {
                await traderRepository.AddRangeAsync(await issBondsRepository.LoadAsync());
            }
        }
    }
}
