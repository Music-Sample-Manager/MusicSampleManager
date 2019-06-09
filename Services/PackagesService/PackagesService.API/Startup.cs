using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.DAL;
using PackagesService.Domain;
using System;

[assembly: FunctionsStartup(typeof(PackagesService.API.Startup))]
namespace PackagesService.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var databaseConnectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");

            // TODO Remove this eventually.
            builder.Services.AddTransient((s) =>
            {
                return new MSMDbContext(false, databaseConnectionString);
            });


            // TODO Why does the API need to know anything about Entity Framework? Code smell - should push some of this into the DAL.
            var databaseOptionsBuilder = new DbContextOptionsBuilder<DbPackageRepository>();
            databaseOptionsBuilder.UseSqlServer(databaseConnectionString);

            IPackageRepository packageRepository = new DbPackageRepository(databaseOptionsBuilder.Options);

            builder.Services.AddTransient((pr) => packageRepository);
        }
    }
}