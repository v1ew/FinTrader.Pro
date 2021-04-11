using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FinTrader.Pro.DB.Data;
using FinTrader.Pro.DB.Repositories;
using FinTrader.Pro.Iss;
using FinTrader.Pro.Bonds.Extensions;
using FinTrader.Pro.Bonds;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Newtonsoft.Json.Converters;

namespace FinTrader.Pro.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // Debug sql queries
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<FinTraderDataContext>(opts => {
                    opts.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]);
                    // Debug sql queries
                    opts.UseLoggerFactory(MyLoggerFactory);
                })
                .AddScoped<IFinTraderRepository, FinTraderRepository>()
                .AddIssHttpClient(Configuration)
                .AddIssBonds()
                .AddScoped<IBondsService, BondsService>()
                .AddControllersWithViews()
                .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter())); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa => {
                //spa.Options.SourcePath = "../src/ClientApp";
                //spa.UseAngularCliServer("start");
                string strategy = Configuration.GetValue<string>("DevTools:ConnectionStrategy");
                if (strategy == "proxy")
                {
                    spa.UseProxyToSpaDevelopmentServer("http://127.0.0.1:4200");
                }
                else
                {
                    spa.Options.SourcePath = "../ClientApp";
                    spa.UseAngularCliServer("start");
                }
            });
        }
    }
}
