using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PackagesService.DAL;
using System.Threading.Tasks;

namespace PackagesService.API.Packages
{
    public static class GetPackageById
    {
        [FunctionName("GetPackageById")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int packageId = int.Parse(req.Query["packageId"]);

            if (packageId <= 0)
            {
                return new BadRequestObjectResult("Please pass a valid packageId in the query string");
            }

            using (var dbContext = new MSMDbContext())
            {
                var package = dbContext.Packages.Find(packageId);

                if (package == null)
                {
                    return new NotFoundObjectResult(packageId);
                }
                else
                {
                    return new OkObjectResult(package);
                }
            }
        }
    }
}