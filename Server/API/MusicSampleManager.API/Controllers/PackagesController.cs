using Domain;
using Microsoft.AspNetCore.Mvc;
using PackageDatabase;
using System;

namespace MusicSampleManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackagesController(IPackageRepository packageRepostiory)
        {
            _packageRepository = packageRepostiory;
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