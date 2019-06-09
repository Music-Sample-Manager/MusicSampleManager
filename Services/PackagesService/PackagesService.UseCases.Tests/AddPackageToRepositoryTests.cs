using Moq;
using PackagesService.Domain;
using System;
using Xunit;

namespace PackagesService.UseCases.Tests
{
    public class AddPackageToRepositoryTests
    {

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentNullException_WhenPackageRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AddPackageToRepository(null, "test", "Test", 1));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentNullException_WhenPackageNameIsNull()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentNullException>(() => new AddPackageToRepository(repo.Object, null, "Test", 1));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentNullException_WhenPackageDescriptionIsNull()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentNullException>(() => new AddPackageToRepository(repo.Object, "TestPackage", null, 1));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentException_WhenPackageNameIsEmpty()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentException>(() => new AddPackageToRepository(repo.Object, string.Empty, "Test", 1));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentException_WhenPackageDescriptionIsEmpty()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentException>(() => new AddPackageToRepository(repo.Object, "TestPackage", string.Empty, 1));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentException_WhenAuthorIdIs0()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentException>(() => new AddPackageToRepository(repo.Object, "TestPackage", "Test description", 0));
        }

        [Fact]
        public void AddPackageToRepository_ThrowsArgumentException_WhenAuthorIdIsNegative()
        {
            var repo = new Mock<IPackageRepository>();

            Assert.Throws<ArgumentException>(() => new AddPackageToRepository(repo.Object, "TestPackage", "Test description", -1));
        }
    }
}
