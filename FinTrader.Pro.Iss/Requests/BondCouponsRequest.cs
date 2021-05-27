using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondCouponsRequest : RequestBase
    {
        public BondCouponsRequest(IIssClient client) : base(client) { }

        public async Task<BondCouponsResponse> FetchAsync(string sec, IDictionary<string, string> args)
        {
            var url = $"iss/securities/{sec}/bondization.json";
            return await IssClient.GetAsync<BondCouponsResponse>(url, args);
        }
    }
}
