using Microsoft.AspNetCore.Mvc;
using PackagesService.Domain;
using System;
using System.IO.Compression;

namespace PublicWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageZipsController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackageZipsController(IPackageRepository packageRepostiory)
        {
            _packageRepository = packageRepostiory;
        }

        public ActionResult<ZipArchive> Get([FromQuery] string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException();
            }

            if (packageName == string.Empty)
            {
                throw new ArgumentException();
            }

            return _packageRepository.DownloadLatestByName(packageName);
        }
    }
}
