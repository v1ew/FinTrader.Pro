﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace FinTrader.Pro.Iss
{
    public static class IssClientExtensions
    {
        public static IServiceCollection AddIssHttpClient(this IServiceCollection serviceCollection/*, IConfiguration configuration*/)
        {
            var issBaseUrl = "https://iss.moex.com/"; //configuration.GetSection("Iss:BaseUrl").Value;
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
