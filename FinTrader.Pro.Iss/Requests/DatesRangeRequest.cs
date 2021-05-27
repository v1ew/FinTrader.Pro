using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class DatesRangeRequest : RequestBase
    {
        public DatesRangeRequest(IIssClient client) : base(client) { }

        public async Task<DatesRangeResponse> FetchAsync(IDictionary<string, string> args)
        {
            const string url = "iss/history/engines/stock/markets/bonds/dates.json";
            return await IssClient.GetAsync<DatesRangeResponse>(url, args);
        }
    }
}