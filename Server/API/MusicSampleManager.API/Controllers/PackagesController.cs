using Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MusicSampleManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        public ActionResult<Package> Get([FromQuery] string packageName)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException();
            }

            return new Package(packageName);
        }
    }
}