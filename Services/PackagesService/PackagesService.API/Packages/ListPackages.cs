using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PackagesService.DAL;
using System.Linq;

namespace PackagesService.API.Packages
{
    public class ListPackages
    {
        private readonly MSMDbContext _dbContext;

        public ListPackages(MSMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName(nameof(ListPackages))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("'List Packages' triggered.");

            int authorId = int.Parse(req.Query["authorId"]);

            if (authorId <= 0)
            {
                return new BadRequestObjectResult("Please pass an authorId in the query string");
            }

            var authorPackages = _dbContext.Packages.Where(p => p.AuthorId == authorId).ToList();

            return (ActionResult)new OkObjectResult(authorPackages);
        }
    }
}