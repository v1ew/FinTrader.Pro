using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class MarketSecuritiesListRequest
    {
        private IIssClient issClient;

        public MarketSecuritiesListRequest(IIssClient client)
        {
            issClient = client;
        }

        public async Task<MarketSecuritiesListResponse> FetchAsync(string engine, string market, IDictionary<string, string> args)
        {
            var url = $"iss/engines/{engine}/markets/{market}/securities.json";
            return await issClient.GetAsync<MarketSecuritiesListResponse>(url, args);
        }
    }
}
