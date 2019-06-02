using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PackagesService.DAL;

[assembly: FunctionsStartup(typeof(PackagesService.API.Startup))]
namespace PackagesService.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient((s) =>
            {
                return new MSMDbContext(false);
            });
        }
    }
}