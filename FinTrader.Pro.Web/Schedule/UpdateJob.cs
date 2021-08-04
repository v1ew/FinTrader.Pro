using System;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinTrader.Pro.Web.Schedule
{
    public class UpdateJob : IJob
    {
        private readonly IServiceProvider provider;

        public UpdateJob(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
        }
        
        public async void Execute()
        {
            ILogger<UpdateJob> logger = null;
            var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var bondsService = scope.ServiceProvider.GetService<IBondsService>();
                var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();

                if (loggerFactory != null)
                {
                    logger = loggerFactory.CreateLogger<UpdateJob>();
                }
            
                try
                {
                    await bondsService.UpdateBondsAsync();
                    await bondsService.DiscardWrongBondsAsync();
                    await bondsService.UpdateCouponsAsync();
                    await bondsService.CheckCouponsAsync();
                    await bondsService.UpdateBondsDurationAsync();
                    await bondsService.UpdateBondsValueAsync();
                }
                catch (Exception e)
                {
                    logger?.Log(LogLevel.Error, e, "Unable to Update");
                }
            }
        }
    }
}