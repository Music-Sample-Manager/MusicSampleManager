using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PackagesService.API.Packages;
using PackagesService.API.WebAPI.Controllers;
using PackagesService.DAL.Entities;
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
        public void GetPackageByName_WhenValidPackageNameIsProvided_ReturnsPackage()
        {
            const string testPackageName = "Test package name";
            var dbContext = TestFactory.GetInMemoryDbContext();
            dbContext.Packages.Add(new PackageRec
            {
                Id = 1,
                Identifier = testPackageName,
                Description = "TestDescription",
                AuthorId = 1
            });
            dbContext.SaveChanges();
            var sut = new GetPackageByName(dbContext);
            var request = TestFactory.CreateHttpRequest("packageName", testPackageName);

            var response = sut.Run(request, _logger);

            Assert.IsType<OkObjectResult>(response);
            Assert.IsType<PackageRec>((response as OkObjectResult).Value);
            Assert.Equal(testPackageName, ((response as OkObjectResult).Value as PackageRec).Identifier);
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