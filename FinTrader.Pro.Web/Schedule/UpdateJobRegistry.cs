using System;
using FluentScheduler;
using Microsoft.Extensions.Configuration;

namespace FinTrader.Pro.Web.Schedule
{
    public class UpdateJobRegistry : Registry
    {
        public UpdateJobRegistry(IServiceProvider provider, IConfiguration configuration)
        {
            int UpdateTimeHours = 4;
            var hasValue = Int32.TryParse(configuration.GetSection("Schedule:UpdateTimeHours").Value, out UpdateTimeHours);
            NonReentrantAsDefault();
            Schedule(() => new UpdateJob(provider))
                .WithName(nameof(UpdateJob))
                .ToRunOnceAt(UpdateTimeHours, 11)
                .AndEvery(1)
                .Days();
        }
    }
}