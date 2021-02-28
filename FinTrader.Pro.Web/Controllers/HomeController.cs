using FinTrader.Pro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FinTrader.Pro.DB.Repositories;
using FinTrader.Pro.Bonds;

namespace FinTrader.Pro.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFinTraderRepository finTraderRepository;
        private readonly IBondsService bondsService;

        public HomeController(ILogger<HomeController> logger, IFinTraderRepository repo, IBondsService bondsServ)
        {
            _logger = logger;
            finTraderRepository = repo;
            bondsService = bondsServ;
        }

        public async Task<IActionResult> Index()
        {
            await bondsService.UpdateStorage();
            return View(finTraderRepository.Bonds);
        }

        public IActionResult BondsPicker() => View(new BondsPickerViewModel());

        [HttpPost]
        public IActionResult BondsPicker(BondsPickerViewModel bondsPicker)
        {
            return View(bondsPicker);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
