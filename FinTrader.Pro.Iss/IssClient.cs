using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss
{
    public class IssClient : IIssClient
    {
        private IHttpClientFactory httpClientFactory;
        private IConfiguration config;

        public IssClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            config = configuration;
        }

        public async Task<TResponseDto> GetAsync<TResponseDto>(string engine, string market, string args)
        {
            // TODO: Check arguments
            string url = $"http://iss.moex.com/iss/engines/{engine}/markets/{market}/securities.json";
            if (!string.IsNullOrEmpty(args))
            {
                url += $"?{args}";
            }

            using (var httpClient = httpClientFactory.CreateClient("iss"))
            {
                var response = await httpClient.GetAsync(url);

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
