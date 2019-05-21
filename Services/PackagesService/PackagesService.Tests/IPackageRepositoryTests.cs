using Moq;
using NUnit.Framework;
using PackagesService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PackagesService.Tests
{
    public class IPackageRepositoryTests
    {
        private readonly Mock<IPackageRepository> _repository = new Mock<IPackageRepository>();

        [SetUp]
        public void Setup()
        {
            _repository.Setup(r => r.GetAll()).Returns(new List<Package>
                                                       {
                                                           new Package("Test package #1"),
                                                           new Package("Test package #2"),
                                                           new Package("Test package #3"),
                                                       });
        }

        [Test]
        public void GetAll_DoesNotExplode()
        {
            var resultPackages = _repository.Object.GetAll();

            Assert.NotNull(resultPackages);
            Assert.IsNotEmpty(resultPackages);
            Assert.AreEqual(3, resultPackages.Count());

            _repository.VerifyAll();
        }
    }
}