using Moq;
using MusicSampleManager.API.Controllers;
using PackageDatabase;
using System;
using Xunit;

namespace API.Tests
{
    public class PackagesControllerTests
    {
        [Fact]
        public void Get_WithEmptyPackageName_Throws()
        {
            var packageRepo = new Mock<IPackageRepository>();
            var sut = new PackagesController(packageRepo.Object);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => sut.Get(string.Empty));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Get_WithNullPackageName_Throws()
        {
            var packageRepo = new Mock<DbPackageRepository>();
            var sut = new PackagesController(packageRepo.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Get(null));
            packageRepo.VerifyAll();
        }
    }
}