using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PackagesService.Domain;
using Semver;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using Xunit;

namespace DesktopClient.DAL.Tests
{
    public class PackageStoreDataTests
    {
        private readonly ILogger _mockLogger = NullLogger.Instance;

        private const string MockProjectFolderName = "TestData";
        private IDirectoryInfo MockProjectFolder => _fakeFileSystem.DirectoryInfo.FromDirectoryName(MockProjectFolderName);
        private readonly MockFileSystem _fakeFileSystem = new MockFileSystem();

        public PackageStoreDataTests()
        {
            _fakeFileSystem.AddDirectory(MockProjectFolderName);
        }

        #region Ctor
        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenNullRootFolderIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageStoreData(_mockLogger, _fakeFileSystem, null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenNullFileSystemIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageStoreData(_mockLogger, null, MockProjectFolder));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenRootFolderDoesNotExistInFileSystem()
        {
            var fileSystem = new MockFileSystem();

            Assert.Throws<ArgumentException>(() => new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName("FakeDirectoryName")));
        }

        [Fact]
        public void Ctor_DoesNotThrowException_WhenFileSystemAndRootFolderExists()
        {
            var rootFolder = "SomeRootHere";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);

            new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(rootFolder));
        }
        #endregion

        #region PackageRootFolder
        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIsNull()
        {
            var sut = new PackageStoreData(_mockLogger, _fakeFileSystem, MockProjectFolder);

            Assert.Throws<ArgumentNullException>(() => sut.PackageRootFolder(null));
        }

        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIdentifierIsNull()
        {
            var sut = new PackageStoreData(_mockLogger, _fakeFileSystem, MockProjectFolder);

            Assert.Throws<ArgumentNullException>(() => sut.PackageRootFolder(new Package(null)));
        }

        [Fact]
        public void PackageRootFolder_ReturnsCorrectFolderName_WhenSuccessful()
        {
            var rootFolder = "SomeRootFolder";
            var mockPackage = new Package("Some.Package.Identifier.Here");
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(rootFolder);

            var sut = new PackageStoreData(_mockLogger, mockFileSystem, mockFileSystem.DirectoryInfo.FromDirectoryName(rootFolder));

            var result = sut.PackageRootFolder(mockPackage);

            Assert.Equal($"{mockFileSystem.DirectoryInfo.FromDirectoryName("SomeRootFolder").FullName}\\{PackageStoreData.RootFolderName}\\{mockPackage.Identifier}", result);
        }
        #endregion

        #region CreateRootFolder
        [Fact]
        public void CreatePackagesFolder_ThrowsInvalidOperationException_WhenPackagesFolderAlreadyExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockProjectFolderName);
            fileSystem.AddDirectory($"{MockProjectFolderName}\\samplePackages");

            var sut = new PackageStoreData(_mockLogger, fileSystem, MockProjectFolder);

            Assert.Throws<InvalidOperationException>(() => sut.CreateRootFolder());
        }

        [Fact]
        public void CreatePackagesFolder_CreatesPackagesFolder_WhenPackagesFolderDoesNotExist()
        {
            var containingFolderName = "TestFolder";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(containingFolderName);

            var sut = new PackageStoreData(_mockLogger, fileSystem, MockProjectFolder);
            sut.CreateRootFolder();

            var projectRoot = fileSystem.DirectoryInfo.FromDirectoryName($"{MockProjectFolderName}\\{PackageStoreData.RootFolderName}");
            Assert.NotNull(projectRoot);

            var projectFolders = projectRoot.EnumerateDirectories();

            Assert.NotNull(projectFolders);
            Assert.Single(projectFolders);

            Assert.Equal(PackageStoreData.RootFolderName, projectFolders.First().Name);
        }
        #endregion

        #region RootFolderExists
        [Fact]
        public void RootFolderExists_ReturnsFalse_WhenPackagesFolderDoesNotExist()
        {
            var sut = new PackageStoreData(_mockLogger, _fakeFileSystem, _fakeFileSystem.DirectoryInfo.FromDirectoryName(@"C:\"));

            Assert.False(sut.RootFolderExists());
        }

        [Fact]
        public void RootFolderExists_ReturnsTrue_WhenPackagesFolderExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(@"C:\");
            fileSystem.AddDirectory(@"C:\TestData");
            fileSystem.AddDirectory(@"C:\TestData\samplePackages");

            var sut = new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(@"C:\TestData"));

            Assert.True(sut.RootFolderExists());
        }
        #endregion

        #region PackageRevisionFolder
        [Fact]
        public void PackageRevisionFolder_ThrowsArgumentNullException_WhenPackageRevisionIsNull()
        {
            var rootFolder = "TestRootFolder";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);
            var sut = new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(rootFolder));

            Assert.Throws<ArgumentNullException>(() => sut.PackageRevisionFolder(null));
        }

        //TODO // Need to also spend some time consolidating all of my unit tests into xUnit.


       [Fact]
        public void PackageRevisionFolder_ThrowsArgumentException_WhenVersionNumberIsNull()
        {
            var rootFolder = "TestRootFolder";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);
            var sut = new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(rootFolder));

            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                Assert.Throws<ArgumentException>(() => sut.PackageRevisionFolder(new PackageRevision(new Package("Test.identifier"), null, new ZipArchive(zip))));
            }
        }

        [Fact]
        public void PackageRevisionFolder_ReturnsCorrectFolder_WhenSuccessful()
        {
            var rootFolder = "TestRootFolder";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);
            var sut = new PackageStoreData(_mockLogger, fileSystem, fileSystem.DirectoryInfo.FromDirectoryName(rootFolder));
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("Some.Package"),
                                                              new SemVersion(new Version("3.4.5")),
                                                              new ZipArchive(zip));

                var result = sut.PackageRevisionFolder(mockPackageRevision);

                Assert.Equal($"{PackageStoreData.RootFolderName}\\{PackageStoreData.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}", result);
            }

        }
        #endregion
    }
}