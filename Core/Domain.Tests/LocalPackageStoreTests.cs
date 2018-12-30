using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using Xunit;

namespace Domain.Tests
{
    public class LocalPackageStoreTests
    {
        private const string MockProjectFolder = "TestData";
        private readonly MockFileSystem _fakeFileSystem = new MockFileSystem();

        public LocalPackageStoreTests()
        {
            _fakeFileSystem.AddDirectory(MockProjectFolder);
        }

        #region ctor
        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootDirectoryDoesNotExistInFileSystem()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, "SomeNonExistentDirectory")));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(_fakeFileSystem, null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectRootPathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, null)));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootPathHasInvalidFormat()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, string.Empty)));
        }
        #endregion

        #region PackagesFolder
        [Fact]
        public void RootFolderExists_ReturnsFalse_WhenPackagesFolderDoesNotExist()
        {
            var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

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
            var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.AddPackage(null));
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackage_WhenFolderDoesNotAlreadyExist()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              new SemVer.Version("1.2.3"),
                                                              new ZipArchive(zip));
                var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

                sut.AddPackage(mockPackageRevision);

                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
            }
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageAndPackageRevision_WhenFolderDoesNotAlreadyExist()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              new SemVer.Version("1.2.3"),
                                                              new ZipArchive(zip));
                var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

                sut.AddPackage(mockPackageRevision);

                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}"));
            }
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageRevision_WhenFolderForDifferentPackageRevisionAlreadyExists()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var mockPackage = new Package("MSMSamplePackages.SampleOne.PackA");
                var mockExistingPackageRevision = new PackageRevision(mockPackage,
                                                                      new SemVer.Version("5.6.7"),
                                                                      new ZipArchive(zip));
                var mockPackageRevision = new PackageRevision(mockPackage,
                                                              new SemVer.Version("1.2.3"),
                                                              new ZipArchive(zip));
                var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

                sut.AddPackage(mockExistingPackageRevision);
                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockExistingPackageRevision.VersionNumber}"));
                sut.AddPackage(mockPackageRevision);

                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockExistingPackageRevision.VersionNumber}"));
                Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockPackageRevision.VersionNumber}"));
            }
        }

        [Fact]
        public void AddPackage_CreatesFolderForPackageInFileSystem_WhenFolderDoesNotAlreadyExist()
        {
            using (var zip = new MemoryStream(Properties.Resources.mockZip))
            {
                var sut = new LocalPackageStore(_fakeFileSystem,
                                                new LocalProject(_fakeFileSystem, MockProjectFolder));
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              new SemVer.Version("1.2.3"), new ZipArchive(zip));
                sut.AddPackage(mockPackageRevision);

                Assert.True(_fakeFileSystem.Directory.Exists($"{sut.PackageRootFolder(mockPackageRevision.Package)}"));
            }
        }

        //[Fact]
        //public void AddPackage_ExtractsPackageContentsToFileSystem_WhenSuccessful()
        //{
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var sut = new LocalPackageStore(_fakeFileSystem,
        //                                        new LocalProject(_fakeFileSystem, MockProjectFolder));
        //        var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
        //                                                      "1.2.3.4", new ZipArchive(zip));
        //        sut.AddPackage(mockPackageRevision);

        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}"));
        //        var extractedFiles = _fakeFileSystem.Directory.EnumerateFiles($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}");
        //        Assert.Single(extractedFiles);
        //        Assert.Equal("TestFile.txt", extractedFiles.First());
        //    }
        //}
        #endregion

        #region PackageRootFolder
        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIsNull()
        {
            var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.PackageRootFolder(null));
        }

        [Fact]
        public void PackageRootFolder_ThrowsArgumentNullException_WhenPackageIdentifierIsNull()
        {
            var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

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

            Assert.Equal($"{mockFileSystem.DirectoryInfo.FromDirectoryName("SomeRootFolder").FullName}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}", result);
        }
        #endregion
    }
}