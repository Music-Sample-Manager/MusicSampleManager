using DesktopClient.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PackagesService.Domain;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using Xunit;

namespace DesktopClient.Domain.Tests
{
    public class LocalPackageStoreTests
    {
        private const string MockProjectFolderName = "TestData";
        private IDirectoryInfo MockProjectFolder => _fakeFileSystem.DirectoryInfo.FromDirectoryName(MockProjectFolderName);
        private readonly MockFileSystem _fakeFileSystem = new MockFileSystem();
        private readonly ILogger _mockLogger = NullLogger.Instance;

        public LocalPackageStoreTests()
        {
            _fakeFileSystem.AddDirectory(MockProjectFolderName);
        }

        #region ctor
        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootDirectoryDoesNotExistInFileSystem()
        {
            var data = new PackageStoreData(_mockLogger, MockProjectFolder);
            var sut = new LocalPackageStore(data);

            Assert.Throws<ArgumentException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem.DirectoryInfo.FromDirectoryName("SomeNonExistentDirectory"))));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenDataAccessIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectRootPathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem.DirectoryInfo.FromDirectoryName(null))));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootPathHasInvalidFormat()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem.DirectoryInfo.FromDirectoryName(string.Empty))));
        }
        #endregion

        #region AddPackage
        [Fact]
        public void AddPackage_ThrowsArgumentNullException_WhenProvidedPackageIsNull()
        {
            IPackageStore sut = new LocalPackageStore(new PackageStoreData(_mockLogger, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.AddPackage(null));
        }

        //[Fact]
        //public void AddPackage_CreatesFolderForPackage_WhenFolderDoesNotAlreadyExist()
        //{
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
        //                                                      new SemVer.Version("1.2.3"),
        //                                                      new ZipArchive(zip));
        //        var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

        //        sut.AddPackage(mockPackageRevision);

        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
        //    }
        //}

        //[Fact]
        //public void AddPackage_CreatesFolderForPackageAndPackageRevision_WhenFolderDoesNotAlreadyExist()
        //{
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
        //                                                      new SemVer.Version("1.2.3"),
        //                                                      new ZipArchive(zip));
        //        var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

        //        sut.AddPackage(mockPackageRevision);

        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}"));
        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}"));
        //    }
        //}

        //[Fact]
        //public void AddPackage_CreatesFolderForPackageRevision_WhenFolderForDifferentPackageRevisionAlreadyExists()
        //{
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var mockPackage = new Package("MSMSamplePackages.SampleOne.PackA");
        //        var mockExistingPackageRevision = new PackageRevision(mockPackage,
        //                                                              new SemVer.Version("5.6.7"),
        //                                                              new ZipArchive(zip));
        //        var mockPackageRevision = new PackageRevision(mockPackage,
        //                                                      new SemVer.Version("1.2.3"),
        //                                                      new ZipArchive(zip));
        //        var sut = new LocalPackageStore(_fakeFileSystem, new LocalProject(_fakeFileSystem, MockProjectFolder));

        //        sut.AddPackage(mockExistingPackageRevision);
        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockExistingPackageRevision.VersionNumber}"));
        //        sut.AddPackage(mockPackageRevision);

        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockExistingPackageRevision.VersionNumber}"));
        //        Assert.True(_fakeFileSystem.Directory.Exists($"{MockProjectFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackage.Identifier}\\{mockPackageRevision.VersionNumber}"));
        //    }
        //}

        //[Fact]
        //public void AddPackage_CreatesFolderForPackageInFileSystem_WhenFolderDoesNotAlreadyExist()
        //{
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var sut = new LocalPackageStore(_fakeFileSystem,
        //                                        new LocalProject(_fakeFileSystem, MockProjectFolder));
        //        var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
        //                                                      new SemVer.Version("1.2.3"), new ZipArchive(zip));
        //        sut.AddPackage(mockPackageRevision);

        //        Assert.True(_fakeFileSystem.Directory.Exists($"{sut.PackageRootFolder(mockPackageRevision.Package)}"));
        //    }
        //}

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
    }
}