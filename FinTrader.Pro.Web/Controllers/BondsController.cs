﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinTrader.Pro.Web.Controllers
{
    /// <summary>
    /// Контроллер для работы с облигациями
    /// </summary>
    [Route("api/bonds")]
    [ApiController]
    public class BondsController : Controller
    {
        private readonly ILogger<HomeController> logger;
        //private readonly IFinTraderRepository finTraderRepository;
        private readonly IIssBondsRepository issBondsRepository;
        private readonly IBondsService bondsService;

        public BondsController(ILogger<HomeController> logger, IIssBondsRepository bondsRepo, IBondsService bondsServ)
        {
            this.logger = logger;
            issBondsRepository = bondsRepo;
            bondsService = bondsServ;
        }

        [HttpGet("update")]
        public async Task<IActionResult> UpdateStorage()
        {
            await bondsService.UpdateStorage();
            return Ok("Ok!");
        }
    }
}
