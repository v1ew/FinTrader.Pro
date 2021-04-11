using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds;
using FinTrader.Pro.Contracts;
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

        [HttpGet("select")]
        public async Task<BondSet> GetBondSet(/*[FromBody] BondsPickerParams picker*/)
        {
            logger.LogDebug("GetBondSet call test");
            return await bondsService.SelectBondsAsync(/*picker*/ new BondsPickerParams());
        }
    }
}
