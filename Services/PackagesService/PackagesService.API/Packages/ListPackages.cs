using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PackagesService.DAL;
using System.Linq;

namespace PackagesService.API.Packages
{
    public static class ListPackages
    {
        [FunctionName(nameof(ListPackages))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("'List Packages' triggered.");

            int authorId = int.Parse(req.Query["authorId"]);

            if (authorId <= 0)
            {
                return new BadRequestObjectResult("Please pass an authorId in the query string");
            }

            using (var dbContext = new MSMDbContext())
            {
                var authorPackages = dbContext.Packages.Where(p => p.AuthorId == authorId).ToList();

                return (ActionResult)new OkObjectResult(authorPackages);
            }
        }
    }
}