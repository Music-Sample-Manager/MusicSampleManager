using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using Xunit;

namespace Domain.Tests
{
    public class LocalProjectTests
    {
        #region Ctor
        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenNullRootFolderIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalProject(new MockFileSystem(), null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenNullFileSystemIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalProject(null, "SomeFolderHere"));
        }

        [Fact]
        public void Ctor_ThrowsArgumentException_WhenRootFolderDoesNotExist()
        {
            Assert.Throws<ArgumentException>(() => new LocalProject(new MockFileSystem(), "SomeRandomFolder"));
        }

        [Fact]
        public void Ctor_DoesNotThrowException_WhenFileSystemAndRootFolderExists()
        {
            var rootFolder = "SomeRootHere";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);

            new LocalProject(fileSystem, rootFolder);
        }
        #endregion

        #region PackageRevisionFolder
        [Fact]
        public void PackageRevisionFolder_ThrowsArgumentNullException_WhenPackageRevisionIsNull()
        {
            var rootFolder = "TestRootFolder";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);
            var sut = new LocalProject(fileSystem, rootFolder);

            Assert.Throws<ArgumentNullException>(() => sut.PackageRevisionFolder(null));
        }



        //[Fact]
        //public void PackageRevisionFolder_ThrowsArgumentException_WhenVersionNumberIsNull()
        //{
        //    var rootFolder = "TestRootFolder";
        //    var fileSystem = new MockFileSystem();
        //    fileSystem.AddDirectory(rootFolder);
        //    var sut = new LocalProject(fileSystem, rootFolder);

        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        Assert.Throws<ArgumentException>(() => sut.PackageRevisionFolder(new PackageRevision(new Package("Test.identifier"), null, new ZipArchive(zip))));
        //    }
        //}

        //[Fact]
        //public void PackageRevisionFolder_ReturnsCorrectFolder_WhenSuccessful()
        //{
        //    var rootFolder = "TestRootFolder";
        //    var fileSystem = new MockFileSystem();
        //    fileSystem.AddDirectory(rootFolder);
        //    var sut = new LocalProject(fileSystem, rootFolder);
        //    using (var zip = new MemoryStream(Properties.Resources.mockZip))
        //    {
        //        var mockPackageRevision = new PackageRevision(new Package("Some.Package"),
        //                                                      new SemVer.Version("3.4.5"),
        //                                                      new ZipArchive(zip));

        //        var result = sut.PackageRevisionFolder(mockPackageRevision);

        //        Assert.Equal($"{sut.RootFolder}\\{LocalPackageStore.RootFolderName}\\{mockPackageRevision.Package.Identifier}\\{mockPackageRevision.VersionNumber}", result);
        //    }

        //}
        #endregion
    }
}