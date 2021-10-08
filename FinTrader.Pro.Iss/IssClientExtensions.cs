using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace FinTrader.Pro.Iss
{
    public static class IssClientExtensions
    {
        public static IServiceCollection AddIssHttpClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var issBaseUrl = configuration.GetSection("Iss:BaseUrl").Value;
            serviceCollection.AddHttpClient("iss", c =>
            {
                c.BaseAddress = new Uri(issBaseUrl);

                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            serviceCollection.AddTransient<IIssClient, IssClient>();

            return serviceCollection;
        }
    }
}
