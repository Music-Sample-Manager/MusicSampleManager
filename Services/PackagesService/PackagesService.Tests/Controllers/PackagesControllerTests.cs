using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using PackagesService.API.Packages;
using PackagesService.API.WebAPI.Controllers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PackagesService.Tests.Controllers
{
    public class PackagesControllerTests
    {
        private PackagesController _packagesController;
        private ILogger _logger;

        public PackagesControllerTests()
        {
            _packagesController = new PackagesController();
            _logger = NullLoggerFactory.Instance.CreateLogger("Test");
        }

        [Fact]
        public async Task GetPackageByName_WhenValidPackageNameIsProvided_ReturnsPackage()
        {
            const string testPackageName = "Test package name";
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var queryParams = new Dictionary<string, StringValues>()
            {
                {
                    "packageName", testPackageName
                }
            };
            request.Query = new QueryCollection(queryParams);
            

            var response = (OkObjectResult) await GetPackageByName.Run(request, _logger);

            Thread.Sleep(3000);

            //Assert.IsInstanceOf<OkObjectResult>(result);
            //Assert.AreEqual(testPackageName, (result as OkObjectResult).Value);
            Assert.Equal(testPackageName, response.Value);
        }

        [Fact]
        public void Get_WhenPackageNameIsNull_Returns400()
        {
            const string testPackageName = null;

            var result = _packagesController.Get(testPackageName);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Get_WhenPackageNameIsEmpty_Returns400()
        {
            string testPackageName = string.Empty;

            var result = _packagesController.Get(testPackageName);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}