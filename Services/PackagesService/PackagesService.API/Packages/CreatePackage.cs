using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PackagesService.DAL;
using PackagesService.DAL.Entities;

namespace PackagesService.API.Packages
{
    public static class CreatePackage
    {
        // * Store content in a Blob - make note of its ID
        // * Add metadata to relational data store, including the Blob ID
        // Data:
        // * Blob content
        // * Package name
        // * Package revision
        [FunctionName(nameof(CreatePackage))]
        public static ActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, ILogger log)
        {
            string packageName = req.Query["packageName"];
            string packageDescription = req.Query["packageDescription"];
            int authorId = int.Parse(req.Query["authorId"]);

            if (packageName == null)
            {
                return new BadRequestObjectResult("Please pass a packageName in the query string");
            }
            if (packageDescription == null)
            {
                return new BadRequestObjectResult("Please pass a packageDescription in the query string");
            }
            if (authorId <= 0)
            {
                return new BadRequestObjectResult("Please pass an authorId in the query string");
            }


            using (var dbContext = new MSMDbContext())
            {
                dbContext.Packages.Add(new PackageRec()
                {
                    Identifier = packageName,
                    Description = packageDescription,
                    AuthorId = authorId
                });

                dbContext.SaveChanges();
            }

            return new OkResult();
        }
    }
}