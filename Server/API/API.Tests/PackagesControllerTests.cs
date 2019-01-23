using Moq;
using PackageDatabase;
using PublicWebsite;
using PublicWebsite.APIClient;
using System;
using Xunit;

namespace API.Tests
{
    public class PackagesControllerTests
    {
        private readonly APIClient client = new APIClient();


        [Fact]
        public void Get_WithEmptyPackageName_Throws()
        {
            var packageRepo = new Mock<IPackageRepository>();
            var sut = new PackagesController(client, packageRepo.Object);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => sut.Get(string.Empty));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Get_WithNullPackageName_Throws()
        {
            var packageRepo = new Mock<IPackageRepository>();
            var sut = new PackagesController(client, packageRepo.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Get(null));
            packageRepo.VerifyAll();
        }
    }
}