using DesktopClient.DAL;
using DesktopClient.Domain.Tests.Properties;
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
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem, _fakeFileSystem.DirectoryInfo.FromDirectoryName("SomeNonExistentDirectory"))));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenDataAccessIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenProjectRootPathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem, _fakeFileSystem.DirectoryInfo.FromDirectoryName(null))));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenProjectRootPathHasInvalidFormat()
        {
            Assert.Throws<ArgumentException>(() => new LocalPackageStore(new PackageStoreData(_mockLogger, _fakeFileSystem, _fakeFileSystem.DirectoryInfo.FromDirectoryName(string.Empty))));
        }
        #endregion

        #region AddPackage
        [Fact]
        public void AddPackage_ThrowsArgumentNullException_WhenPackageRevisionIsNull()
        {
            var sut = new LocalPackageStore(new MockPackageStoreData(_mockLogger, MockProjectFolder));

            Assert.Throws<ArgumentNullException>(() => sut.AddPackageRevision(null));
        }

        [Fact]
        public void AddPackage_AddsPackageAndRevisionToEntries_WhenPackageDoesNotExistInStore()
        {
            var mockDAL = new MockPackageStoreData(_mockLogger, MockProjectFolder);
            var sut = new LocalPackageStore(mockDAL);

            Assert.Empty(sut.Entries);

            using (var zipStream = new MemoryStream(Resources.mockZip))
            {
                var mockPackageRevision = new PackageRevision(new Package("MSMSamplePackages.SampleOne.PackA"),
                                                              new SemVersion(0),
                                                              new ZipArchive(zipStream));

                sut.AddPackageRevision(mockPackageRevision);

                Assert.Single(sut.Entries);
                Assert.Equal("MSMSamplePackages.SampleOne.PackA", sut.Entries[0].Package.Identifier);
            }
        }

        [Fact]
        public void AddPackage_AddsPackageAndRevisionToEntries_WhenPackageDoesExistInStore()
        {
            var mockDAL = new MockPackageStoreData(_mockLogger, MockProjectFolder);
            var mockPackage = new Package("SamplePackage.Something.Else");

            using (var zipStream = new MemoryStream(Resources.mockZip))
            {

                var mockPackageRev1 = new PackageRevision(mockPackage, new SemVersion(0, 1), new ZipArchive(zipStream));
                var mockPackageRev2 = new PackageRevision(mockPackage, new SemVersion(0, 2), new ZipArchive(zipStream));

                var sut = new LocalPackageStore(mockDAL);

                Assert.Empty(sut.Entries);

                sut.AddPackageRevision(mockPackageRev1);

                Assert.Single(sut.Entries);
                Assert.Equal(mockPackage.Identifier, sut.Entries[0].Package.Identifier);

                sut.AddPackageRevision(mockPackageRev2);

                Assert.Single(sut.Entries);
                Assert.Equal(2, sut.Entries[0].PackageRevisions.Count());
            }
        }

        //[Fact]
        //TODO These are DAL-layer tests. They need to be moved to a DAL-testing project.
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