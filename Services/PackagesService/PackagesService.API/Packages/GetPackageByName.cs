using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PackagesService.DAL;
using System.Linq;
using System.Threading.Tasks;

namespace PackagesService.API.Packages
{
    public class GetPackageByName
    {
        private readonly MSMDbContext _dbContext;

        public GetPackageByName(MSMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName(nameof(GetPackageByName))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string packageName = req.Query["packageName"];

            if (string.IsNullOrEmpty(packageName))
            {
                return new BadRequestObjectResult("Please provide a packageName in the query string");
            }

            var package = _dbContext.Packages.SingleOrDefault(p => p.Identifier == packageName);

            if (package == null)
            {
                return new NotFoundObjectResult(packageName);
            }
            else
            {
                return new OkObjectResult(package);
            }
        }
    }
}