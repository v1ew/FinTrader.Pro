using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class SecurityHistoryRequest : RequestBase
    {
        public SecurityHistoryRequest(IIssClient client) : base(client) {}

        public async Task<SecurityHistoryResponse> FetchAsync(string engine, string market, string secId, IDictionary<string, string> args)
        {
            var url = $"iss/history/engines/{engine}/markets/{market}/securities/{secId}.json";
            return await IssClient.GetAsync<SecurityHistoryResponse>(url, args);
        }
    }
}