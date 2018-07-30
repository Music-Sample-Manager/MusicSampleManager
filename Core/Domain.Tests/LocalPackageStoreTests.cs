using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Xunit;

namespace Domain.Tests
{
    public class LocalPackageStoreTests
    {
        private const string TestDataFolderName = "TestData";

        [Fact]
        public void PackagesFolderExists_ReturnsFalse_WhenPackagesFolderDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(TestDataFolderName);

            var sut = new LocalPackageStore(fileSystem, TestDataFolderName);
            Assert.False(sut.PackagesFolderExists());
        }

        [Fact]
        public void PackagesFolderExists_ReturnsTrue_WhenPackagesFolderExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(TestDataFolderName);
            fileSystem.AddDirectory(@"TestData\samplePackages");

            var sut = new LocalPackageStore(fileSystem, TestDataFolderName);
            Assert.True(sut.PackagesFolderExists());
        }


        [Fact]
        public void CreatePackagesFolder_ThrowsInvalidOperationException_WhenPackagesFolderAlreadyExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(TestDataFolderName);
            fileSystem.AddDirectory($"{TestDataFolderName}\\samplePackages");

            var sut = new LocalPackageStore(fileSystem, TestDataFolderName);

            Assert.Throws<InvalidOperationException>(() => sut.CreatePackagesFolder());
        }

        [Fact]
        public void CreatePackagesFolder_CreatesPackagesFolder_WhenPackagesFolderDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(TestDataFolderName);

            var sut = new LocalPackageStore(fileSystem, TestDataFolderName);
            sut.CreatePackagesFolder();

            var projectRoot = fileSystem.DirectoryInfo.FromDirectoryName(TestDataFolderName);
            Assert.NotNull(projectRoot);

            var projectFolders = projectRoot.EnumerateDirectories();

            Assert.NotNull(projectFolders);
            Assert.Single(projectFolders);

            Assert.Equal(LocalPackageStore.PackagesFolderName, projectFolders.First().Name);
        }
    }
}