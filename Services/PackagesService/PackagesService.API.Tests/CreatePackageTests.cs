using Xunit;

namespace PackagesService.API.Tests
{
    public class CreatePackageTests
    {
        [Fact]
        public void CreatePackage_WhenPackageNameIsEmpty_ReturnsBadRequestObjectResult()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreatePackage_WhenPackageNameIsNull_ReturnsBadRequestObjectResult()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreatePackage_WhenPackageDescriptionIsEmpty_ReturnsBadRequestObjectResult()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreatePackage_WhenPackageDescriptionIsNull_ReturnsBadRequestObjectResult()
        {
            Assert.True(false);
        }
    }
}
