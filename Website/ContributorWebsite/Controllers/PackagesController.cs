using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackagesService.Domain;
using System;

namespace PublicWebsite
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly APIClient.APIClient _apiClient;
        private readonly IPackageRepository _packageRepository;

        public PackagesController(APIClient.APIClient apiClient, IPackageRepository packageRepostiory)
        {
            _apiClient = apiClient;
            _packageRepository = packageRepostiory;
        }

        [HttpPost("Create")]
        public ActionResult Create()
        {

            var packageName = Request.Form["PackageName"];
            var packageDescription = Request.Form["PackageDescription"];
            var packageAuthorId = int.Parse(Request.Form["PackageAuthorId"]);

            _apiClient.CreatePackage(packageName, packageDescription, packageAuthorId);

            return new RedirectToPageResult("/ManagePackages/Index");
        }

        [HttpPost("AddRevision")]
        public ActionResult AddRevision(IFormFile packageRevisionFile)
        {
            var versionNumber = Request.Form["VersionNumber"];

            _apiClient.AddPackageRevision(versionNumber, packageRevisionFile);

            return new OkResult();
        }

        public ActionResult<Package> Get([FromQuery] string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException();
            }

            if (packageName == string.Empty)
            {
                throw new ArgumentException();
            }

            return _packageRepository.FindByName(packageName);
        }
    }
}