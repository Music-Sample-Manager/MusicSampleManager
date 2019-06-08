using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PackagesService.Domain;
using PackagesService.UseCases;

namespace PackagesService.API.Packages
{
    public class CreatePackage
    {
        private readonly IPackageRepository _packageRepository;

        public CreatePackage(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        // * Store content in a Blob - make note of its ID
        // * Add metadata to relational data store, including the Blob ID
        // Data:
        // * Blob content
        // * Package name
        // * Package revision
        [FunctionName(nameof(CreatePackage))]
        public ActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, ILogger log)
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

            var addPackageToRepository = new AddPackageToRepository(_packageRepository, packageName, packageDescription, authorId);
            addPackageToRepository.Execute();            

            return new OkResult();
        }
    }
}