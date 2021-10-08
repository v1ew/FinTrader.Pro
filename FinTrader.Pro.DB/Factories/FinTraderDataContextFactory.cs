using System;
using FinTrader.Pro.DB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace FinTrader.Pro.DB.Factories
{
    public class FinTraderDataContextFactory : IDesignTimeDbContextFactory<FinTraderDataContext>
    {
        private const string MainApplicationProjectName = "FinTrader.Pro.Web";
        private const string MySqlServerVersion = "10.6.4";

        public FinTraderDataContext CreateDbContext(string[] args)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath($"{projectPath}..\\{MainApplicationProjectName}\\")
                .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FinTraderDataContext>();
            optionsBuilder.UseMySql(new MySqlConnection(connectionString), ServerVersion.Parse(MySqlServerVersion));

            return new FinTraderDataContext(optionsBuilder.Options);
        }
    }
}
