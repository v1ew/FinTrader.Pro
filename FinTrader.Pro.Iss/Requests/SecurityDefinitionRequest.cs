using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss.Requests
{
    public class SecurityDefinitionRequest
    {
        private IIssClient _issClient;

        public SecurityDefinitionRequest(IIssClient issClient)
        {
            _issClient = issClient;
        }

        public async Task<SecurityDefinitionResponse> FetchAsync(string secId, IDictionary<string, string> args)
        {
            var url = $"iss/securities/{secId}.json";
            return await _issClient.GetAsync<SecurityDefinitionResponse>(url, args);
        }
    }
}