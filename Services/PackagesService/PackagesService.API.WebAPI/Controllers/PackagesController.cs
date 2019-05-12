using Microsoft.AspNetCore.Mvc;

namespace PackagesService.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Get(string packageName)
        {
            if (packageName == null || packageName == string.Empty)
            {
                return new BadRequestObjectResult("packageName cannot be null or empty.");
            }

            return new OkObjectResult(packageName);
        }
    }
}