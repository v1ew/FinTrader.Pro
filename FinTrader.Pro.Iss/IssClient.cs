using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss
{
    public class IssClient : IIssClient
    {
        private IHttpClientFactory httpClientFactory;

        public IssClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<TResponseDto> GetAsync<TResponseDto>(string url, IDictionary<string, string> args = null)
        {
            using (var httpClient = httpClientFactory.CreateClient("iss"))
            {
                var response = await httpClient.GetAsync(url + (args != null ? "?" + string.Join("&", args.Select(a => $"{a.Key}={a.Value}")) : ""));

                if (response?.IsSuccessStatusCode == true)
                {
                    var result = JsonConvert.DeserializeObject<TResponseDto>(await response.Content.ReadAsStringAsync());
                    return result;
                }

                throw new HttpRequestException($"IssClient: Data retrieving error. StatusCode = {response?.StatusCode}.");
            }
        }
    }
}
