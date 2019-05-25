using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PackagesService.API.Packages
{
    public static class GetPackageByName
    {
        [FunctionName(nameof(GetPackageByName))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string packageName = req.Query["packageName"];

            if (string.IsNullOrEmpty(packageName))
            {
                return new BadRequestObjectResult("Please provide a packageName in the query string");
            }

            //using (var dbContext = new MSMDbContext())
            //{
            //    var package = dbContext.Packages.SingleOrDefault(p => p.Identifier == packageName);

            //    if (package == null)
            //    {
            //        return new NotFoundObjectResult(packageName);
            //    }
            //    else
            //    {
            //        return new OkObjectResult(package);
            //    }
            //}
            return new OkObjectResult("Waiting for new functions runtime...");
        }
    }
}