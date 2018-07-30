using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Domain.Tests
{
    public class LocalPackageStoreTests
    {
        [Fact]
        public void PackagesFolderExists_ReturnsFalse_WhenPackagesFolderDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory("TestData");

            var sut = new LocalPackageStore(fileSystem, "TestData");
            Assert.False(sut.PackagesFolderExists());
        }

        [Fact]
        public void PackagesFolderExists_ReturnsTrue_WhenPackagesFolderExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory("TestData");
            fileSystem.AddDirectory(@"TestData\samplePackages");

            var sut = new LocalPackageStore(fileSystem, "TestData");
            Assert.True(sut.PackagesFolderExists());
        }
    }
}