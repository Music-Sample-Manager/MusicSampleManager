using Moq;
using PackagesService.Domain;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PackagesService.Tests
{
    public class IPackageRepositoryTests
    {
        private readonly Mock<IPackageRepository> _repository = new Mock<IPackageRepository>();

        public IPackageRepositoryTests()
        {
            _repository.Setup(r => r.GetAll()).Returns(new List<Package>
                                                       {
                                                           new Package(0, "Test package #1", string.Empty, 0),
                                                           new Package(0, "Test package #2", string.Empty, 0),
                                                           new Package(0, "Test package #3", string.Empty, 0),
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