using Moq;
using PackagesService.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace PackagesService.UseCases.Tests
{
    public class ListAllPackagesForAuthorTests
    {
        [Fact]
        public void ListAllPackagesForAuthor_WhenRepositoryIsNull_ThrowsArgumentNullException()
        {
            var sut = new ListAllPackagesForAuthor();

            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, 1));
        }

        [Fact]
        public void ListAllPackagesForAuthor_WhenAuthorIdIs0_ThrowsArgumentException()
        {
            var repo = new Mock<IPackageRepository>();
            var sut = new ListAllPackagesForAuthor();

            Assert.Throws<ArgumentException>(() => sut.Execute(repo.Object, 0));
        }

        [Fact]
        public void ListAllPackagesForAuthor_WhenAuthorIdIsNegative_ThrowsArgumentException()
        {
            var repo = new Mock<IPackageRepository>();
            var sut = new ListAllPackagesForAuthor();

            Assert.Throws<ArgumentException>(() => sut.Execute(repo.Object, -1));
        }

        [Fact]
        public void ListAllPackagesForAuthor_WhenNoPackagesAreInRepository_ReturnsEmptyList()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>());
            var sut = new ListAllPackagesForAuthor();

            var result = sut.Execute(repo.Object, 1);

            Assert.Empty(result);
        }

        [Fact]
        public void ListAllPackagesForAuthor_When1PackageMatchesAuthorId_ReturnsListWith1Item()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>()
                         {
                            new Package(0, "Test.Package.1", "Here is a description", 1),
                            new Package(0, "Test.Package.2", "Here is a description", 2),
                            new Package(0, "Test.Package.3", "Here is a description", 3)
                         });
            var sut = new ListAllPackagesForAuthor();

            var result = sut.Execute(repo.Object, 1);

            Assert.Single(result);
        }

        [Fact]
        public void ListAllPackagesForAuthor_WhenMultiplePackagesMatchAuthorId_ReturnsListWithMultipleItems()
        {
            var repo = new Mock<IPackageRepository>();
            repo.Setup((r) => r.GetAll())
                .Returns(new List<Package>()
                         {
                            new Package(0, "Test.Package.1", "Here is a description", 1),
                            new Package(0, "Test.Package.2", "Here is a description", 2),
                            new Package(0, "Test.Package.3", "Here is a description", 1),
                            new Package(0, "Test.Package.4", "Here is a description", 4),
                            new Package(0, "Test.Package.5", "Here is a description", 1),
                         });
            var sut = new ListAllPackagesForAuthor();

            var result = sut.Execute(repo.Object, 1);

            Assert.Equal(3, result.Count);
        }
    }
}