using Microsoft.Extensions.DependencyInjection;

namespace FinTrader.Pro.Bonds.Extensions
{
    public static class BondsExtensions
    {
        public static IServiceCollection AddIssBonds(this IServiceCollection serviceCollection/*, IConfiguration configuration*/)
        {
            serviceCollection.AddTransient<IIssBondsRepository, IssBondsRepository>();

            return serviceCollection;
        }
    }
}
