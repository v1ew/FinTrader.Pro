﻿using FinTrader.Pro.Web.Models;
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
        private readonly IIssBondsRepository issBondsRepository;
        private readonly IBondsService bondsService;

        public HomeController(ILogger<HomeController> logger, IIssBondsRepository bondsRepo, IFinTraderRepository repo, IBondsService bondsServ)
        {
            _logger = logger;
            finTraderRepository = repo;
            issBondsRepository = bondsRepo;
            bondsService = bondsServ;
        }

        public async Task<IActionResult> Index()
        {
            //await bondsService.UpdateStorage();

            return View(await issBondsRepository.LoadBondsAsync());
        }

        public IActionResult BondsPicker() => View(new BondsPickerViewModel());

        [HttpPost]
        public IActionResult BondsPicker(BondsPickerViewModel bondsPicker)
        {
            var data = bondsPicker;
            return View(bondsPicker);
        }

        public async Task<IActionResult> ClearCache()
        {
            await finTraderRepository.ClearCacheAsync();
            return Ok("OK");
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
