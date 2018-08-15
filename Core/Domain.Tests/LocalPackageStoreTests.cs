using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Xunit;

namespace Domain.Tests
{
    public class LocalPackageStoreTests
    {
        private const string MockProjectFolder = "TestData";
        private readonly MockFileSystem _readonlyMockFileSystem = new MockFileSystem();

        public LocalPackageStoreTests()
        {
            _readonlyMockFileSystem.AddDirectory(MockProjectFolder);
        }

        #region ctor
        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootDirectoryDoesNotExistInFileSystem()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, "SomeNonExistentDirectory")));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(_readonlyMockFileSystem, null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectRootPathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, null)));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootPathHasInvalidFormat()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, string.Empty)));
        }
        #endregion

        #region PackagesFolder
        [Fact]
        public void RootFolderExists_ReturnsFalse_WhenPackagesFolderDoesNotExist()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            Assert.False(sut.RootFolderExists());
        }

        [Fact]
        public void RootFolderExists_ReturnsTrue_WhenPackagesFolderExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockProjectFolder);
            fileSystem.AddDirectory(@"TestData\samplePackages");

            var sut = new LocalPackageStore(fileSystem, new LocalProject(fileSystem, MockProjectFolder));

            Assert.True(sut.RootFolderExists());
        }
        #endregion

        #region CreateRootFolder
        [Fact]
        public void CreatePackagesFolder_ThrowsInvalidOperationException_WhenPackagesFolderAlreadyExists()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockProjectFolder);
            fileSystem.AddDirectory($"{MockProjectFolder}\\samplePackages");

            var sut = new LocalPackageStore(fileSystem, new LocalProject(fileSystem, MockProjectFolder));

            Assert.Throws<InvalidOperationException>(() => sut.CreateRootFolder());
        }

        [Fact]
        public void CreatePackagesFolder_CreatesPackagesFolder_WhenPackagesFolderDoesNotExist()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(MockProjectFolder);

            var sut = new LocalPackageStore(fileSystem, new LocalProject(fileSystem, MockProjectFolder));
            sut.CreateRootFolder();

            var projectRoot = fileSystem.DirectoryInfo.FromDirectoryName(MockProjectFolder);
            Assert.NotNull(projectRoot);

            var projectFolders = projectRoot.EnumerateDirectories();

            Assert.NotNull(projectFolders);
            Assert.Single(projectFolders);

            Assert.Equal(LocalPackageStore.RootFolderName, projectFolders.First().Name);
        }
        #endregion

        #region AddPackage
        [Fact]
        public void AddPackage_ThrowsArgumentNullException_WhenProvidedPackageIsNull()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.AddPackage(null));
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackage_WhenFolderDoesNotAlreadyExist()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              "1.2.3.4",
                                                              new ZipArchive(zip));
                var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

                sut.AddPackage(mockPackageRevision);

                Assert.True(_readonlyMockFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
            }
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageAndPackageRevision_WhenFolderDoesNotAlreadyExist()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              "1.2.3.4",
                                                              new ZipArchive(zip));
                var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

                sut.AddPackage(mockPackageRevision);

                Assert.True(_readonlyMockFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
                Assert.True(_readonlyMockFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}"));
            }
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageRevision_WhenFolderForDifferentPackageRevisionIsAlreadyInstalled()
        {
            // TODO This seems more like an integration test.
            Assert.True(false);
        }


        [Fact]
        public void AddPackage_DoesNotTouchDestinationFolder_WhenRequestedPackageIsAlreadyInstalled()
        {
            var mockFolder = $"SomeProject\\{LocalPackageStore.RootFolderName}\\TestPackage\\1.2.3.4";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(mockFolder);

            var expectedEditTime = fileSystem.DirectoryInfo.FromDirectoryName(mockFolder).LastWriteTime;

            var sut = new LocalPackageStore(fileSystem, new LocalProject(fileSystem, "SomeProject"));

            sut.AddPackage(new PackageRevision(new Package("TestPackage"), "1.2.3.4", new ZipArchive(new MemoryStream())));

            var actualEditTime = fileSystem.DirectoryInfo.FromDirectoryName(mockFolder).LastWriteTime;

            Assert.Equal(expectedEditTime, actualEditTime);
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageInFileSystem_WhenFolderDoesNotAlreadyExist()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem,
                                            new LocalProject(_readonlyMockFileSystem, MockProjectFolder));
            var mockPackageContents = new MemoryStream(Encoding.UTF8.GetBytes("SomeTestContentHere"));
            var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"), "1.2.3.4", new ZipArchive(mockPackageContents));
            sut.AddPackage(mockPackageRevision);

            Assert.True(_readonlyMockFileSystem.Directory.Exists($"{sut.PackageRootFolder(mockPackageRevision.Package)}"));
        }

        [Fact]
        public void AddPackage_ExtractsPackageContentsToFileSystem_WhenSuccessful()
        {
            Assert.True(false);
        }
        #endregion

        #region PackageRootFolder
        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIsNull()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.PackageRootFolder(null));
        }

        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIdentifierIsNull()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.PackageRootFolder(new Package(null)));
        }

        [Fact]
        public void PackageRootFolder_ReturnsCorrectFolderName_WhenSuccessful()
        {
            var rootFolder = "SomeRootFolder";
            var mockPackage = new Package("Some.Package.Identifier.Here");
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(rootFolder);

            var sut = new LocalPackageStore(mockFileSystem, new LocalProject(mockFileSystem, rootFolder));

            var result = sut.PackageRootFolder(mockPackage);

            Assert.Equal($"{mockFileSystem.DirectoryInfo.FromDirectoryName("SomeRootFolder")}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}", result);
        }
        #endregion

        #region PackageRevisionFolder
        [Fact]
        public void PackageRevisionFolder_ThrowsArgumentNullException_WhenPackageRevisionIsNull()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.PackageRevisionFolder(null));
        }

        [Fact]
        public void PackageRevisionFolder_ThrowsArgumentException_WhenVersionNumberIsNull()
        {
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                Assert.Throws<ArgumentException>(() => sut.PackageRevisionFolder(new PackageRevision(new Package("Test.identifier"), null, new ZipArchive(zip))));
            }
        }

        [Fact]
        public void PackageRevisionFolder_ReturnsCorrectValue_WhenSuccessful()
        {
            var expectedResult = @"TestData\samplePackages\Test.identifier\1.2.3.4";
            var sut = new LocalPackageStore(_readonlyMockFileSystem, new LocalProject(_readonlyMockFileSystem, MockProjectFolder));

            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("Test.identifier"), "1.2.3.4", new ZipArchive(zip));

                var result = sut.PackageRevisionFolder(mockPackageRevision);

                Assert.Equal(expectedResult, result);
            }
        }
        #endregion
    }
}