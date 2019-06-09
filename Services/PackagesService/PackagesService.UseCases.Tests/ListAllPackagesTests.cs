using Moq;
using PackagesService.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace PackagesService.UseCases.Tests
{
    public class ListAllPackagesTests
    {
        [Fact]
        public void ListAllPackages_WhenRepositoryIsNull_ThrowsArgumentNullException()
        {
            var sut = new ListAllPackages();

            Assert.Throws<ArgumentNullException>(() => sut.Execute(null));
        }

        [Fact]
        public void ListAllPackages_WhenNoPackagesAreInRepository_ReturnsEmptyList()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>());
            var sut = new ListAllPackages();

            var result = sut.Execute(repo.Object);

            Assert.Empty(result);
        }

        [Fact]
        public void ListAllPackages_When1PackageIsInRepository_ReturnsListWith1Item()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>()
                         {
                            new Package(0, "Test.Package", "Here is a description", 1)
                         });
            var sut = new ListAllPackages();

            var result = sut.Execute(repo.Object);

            Assert.Single(result);
        }

        [Fact]
        public void ListAllPackages_WhenMultiplePackagesAreInRepository_ReturnsListWithMultipleItems()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>()
                         {
                            new Package(0, "Test.Package.1", "Here is a description", 1),
                            new Package(0, "Test.Package.2", "Here is a description", 2),
                            new Package(0, "Test.Package.3", "Here is a description", 3),
                            new Package(0, "Test.Package.4", "Here is a description", 4),
                            new Package(0, "Test.Package.5", "Here is a description", 5),
                         });
            var sut = new ListAllPackages();

            var result = sut.Execute(repo.Object);

            Assert.Equal(5, result.Count);
        }
    }
}