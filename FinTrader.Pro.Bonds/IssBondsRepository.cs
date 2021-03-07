﻿using FinTrader.Pro.DB.Models;
using FinTrader.Pro.Iss;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using FinTrader.Pro.Iss.Requests;

namespace FinTrader.Pro.Bonds
{
    public class IssBondsRepository : IIssBondsRepository
    {
        private IIssClient issClient;

        public IssBondsRepository(IIssClient client)
        {
            issClient = client;
        }

        public async Task<List<Dictionary<string, string>>> LoadAsync()
        {
            var request = new MarketSecuritiesListRequest(issClient);
            var bonds = await request.FetchAsync("stock", "bonds", new Dictionary<string, string> { 
                { "iss.only", "securities" },
                { "iss.meta", "off" },
                { "iss.df", "%d-%m-%Y&iss.tf=%H:%M:%S" }
            });
            //var bonds = await issClient.GetAsync<Models.Bonds>("stock", "bonds", "iss.only=securities&iss.meta=off&&iss.df=%d-%m-%Y&iss.tf=%H:%M:%S");

            return bonds.Securities.Data;
        }
    }
}
