using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.Contracts.Bonds;
using FinTrader.Pro.DB.Models;
using FinTrader.Pro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinTrader.Pro.Web.Controllers
{
    /// <summary>
    /// Контроллер для подбора облигаций
    /// </summary>
    [Route("api/bondsets")]
    [ApiController]
    public class BondSetsController : Controller
    {
        private readonly ILogger<BondSetsController> logger;
        private readonly IBondsService bondsService;

        public BondSetsController(ILogger<BondSetsController> logger, IBondsService bondsService)
        {
            this.logger = logger;
            this.bondsService = bondsService;
        }

        [HttpPost]
        public async Task<Portfolio[]> GetBondSet([FromBody] BondsPickerParams picker)
        {
            if (!(picker.IsIncludedCorporate || picker.IsIncludedFederal))
            {
                return new [] { 
                    new Portfolio
                    {
                        BondSets = new List<BondSet> {
                            new BondSet {Bonds = new SelectedBond[] {}},
                            new BondSet {Bonds = new SelectedBond[] {}},
                        }
                    }
                };
            }

            return await bondsService.SelectBondsAsync(picker);
        }
    }
}
