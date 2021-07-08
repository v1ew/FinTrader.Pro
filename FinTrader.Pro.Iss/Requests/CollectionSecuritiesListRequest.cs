using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class CollectionSecuritiesListRequest : RequestBase
    {
        public CollectionSecuritiesListRequest(IIssClient client) : base(client) { }

        public async Task<CollectionSecuritiesListResponse> FetchAsync(string securityGroup, string collection, IDictionary<string, string> args)
        {
            var url = $"iss/securitygroups/{securityGroup}/collections/{collection}/securities.json";
            return await IssClient.GetAsync<CollectionSecuritiesListResponse>(url, args);
        }
    }
}