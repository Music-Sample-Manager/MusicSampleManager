using Moq;
using PackagesService.Domain;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PackageDatabase.Tests
{
    public class PackageDatabaseTests
    {
        private readonly Mock<IPackageRepository> _repository = new Mock<IPackageRepository>();

        public PackageDatabaseTests()
        {
            _repository.Setup(r => r.GetAll()).Returns(new List<Package>
                                                       {
                                                           new Package("Test package #1"),
                                                           new Package("Test package #2"),
                                                           new Package("Test package #3"),
                                                       });
        }

        [Fact]
        public void GetAll_DoesNotExplode()
        {
            var resultPackages = _repository.Object.GetAll();

            Assert.NotNull(resultPackages);
            Assert.NotEmpty(resultPackages);
            Assert.Equal(3, resultPackages.Count());

            _repository.VerifyAll();
        }
    }
}