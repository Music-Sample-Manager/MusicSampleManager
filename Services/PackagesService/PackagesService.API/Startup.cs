using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.DAL;
using System;

[assembly: FunctionsStartup(typeof(PackagesService.API.Startup))]
namespace PackagesService.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            builder.Services.AddTransient((s) =>
            {
                return new MSMDbContext(false, connectionString);
            });
        }
    }
}