using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondsDurationsRequest
    {
        private IIssClient _issClient;

        public BondsDurationsRequest(IIssClient client)
        {
            _issClient = client;
        }

        /// <summary>
        /// Получить дюрации и доходности
        /// </summary>
        /// <param name="args">Date required</param>
        /// <returns></returns>
        public async Task<BondCouponsResponse> FetchAsync(IDictionary<string, string> args)
        {
            var url = "iss/statistics/engines/stock/markets/bonds/durations.json";
            return await _issClient.GetAsync<BondCouponsResponse>(url, args);
        }
    }
}