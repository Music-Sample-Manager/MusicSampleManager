using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Linq;
using PackagesService.DAL;
using Microsoft.Extensions.Logging;

namespace PackagesService.API.PackageRevisions
{
    public static class GetPackageRevisionsByPackageId
    {
        [FunctionName(nameof(GetPackageRevisionsByPackageId))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int packageId;
            if (!int.TryParse(req.Query["packageId"], out packageId))
            {
                return new BadRequestObjectResult("Please pass a packageId in the query string");
            }

            using (var dbContext = new MSMDbContext(false))
            {
                var packageRevisions = dbContext.PackageRevisions.Where(pr => pr.PackageId == packageId).ToList();

                return new OkObjectResult(packageRevisions);
            }
        }
    }
}