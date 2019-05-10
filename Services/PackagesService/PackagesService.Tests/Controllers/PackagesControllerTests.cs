using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PackagesService.API.Controllers;

namespace PackagesService.Tests.Controllers
{
    class PackagesControllerTests
    {
        private PackagesController _packagesController;

        [SetUp]
        public void Setup()
        {
            _packagesController = new PackagesController();
        }

        [Test]
        public void Get_WhenValidPackageNameIsProvided_ReturnsPackage()
        {
            const string testPackageName = "Test package name";

            var result = _packagesController.Get(testPackageName);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(testPackageName, ((OkObjectResult)result).Value);
        }

        [Test]
        public void Get_WhenPackageNameIsNull_Returns400()
        {
            const string testPackageName = null;

            var result = _packagesController.Get(testPackageName);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Get_WhenPackageNameIsEmpty_Returns400()
        {
            string testPackageName = string.Empty;

            var result = _packagesController.Get(testPackageName);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}