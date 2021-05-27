using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class SecurityDefinitionRequest : RequestBase
    {
        public SecurityDefinitionRequest(IIssClient client) : base(client) { }

        public async Task<SecurityDefinitionResponse> FetchAsync(string secId, IDictionary<string, string> args)
        {
            var url = $"iss/securities/{secId}.json";
            return await IssClient.GetAsync<SecurityDefinitionResponse>(url, args);
        }
    }
}