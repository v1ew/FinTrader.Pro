using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondCouponsRequest
    {
        private IIssClient issClient;

        public BondCouponsRequest(IIssClient client)
        {
            issClient = client;
        }

        public async Task<BondCouponsResponse> FetchAsync(string sec, IDictionary<string, string> args)
        {
            var url = $"iss/securities/{sec}/bondization.json";
            return await issClient.GetAsync<BondCouponsResponse>(url, args);
        }
    }
}
