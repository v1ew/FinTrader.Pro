using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondsDurationsRequest : RequestBase
    {
        public BondsDurationsRequest(IIssClient client) : base(client) { }

        /// <summary>
        /// Получить дюрации и доходности
        /// </summary>
        /// <param name="args">Date required</param>
        /// <returns></returns>
        public async Task<BondsDurationsResponse> FetchAsync(IDictionary<string, string> args)
        {
            const string url = "iss/statistics/engines/stock/markets/bonds/durations.json";
            return await IssClient.GetAsync<BondsDurationsResponse>(url, args);
        }
    }
}