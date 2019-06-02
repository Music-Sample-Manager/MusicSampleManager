using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Linq;
using PackagesService.DAL;
using Microsoft.Extensions.Logging;

namespace PackagesService.API.PackageRevisions
{
    public class GetPackageRevisionsByPackageId
    {
        private readonly MSMDbContext _dbContext;

        public GetPackageRevisionsByPackageId(MSMDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [FunctionName(nameof(GetPackageRevisionsByPackageId))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int packageId;
            if (!int.TryParse(req.Query["packageId"], out packageId))
            {
                return new BadRequestObjectResult("Please pass a packageId in the query string");
            }

            var packageRevisions = _dbContext.PackageRevisions.Where(pr => pr.PackageId == packageId).ToList();

            return new OkObjectResult(packageRevisions);
        }
    }
}