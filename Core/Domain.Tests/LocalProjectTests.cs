using System;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Domain.Tests
{
    public class LocalProjectTests
    {
        #region Ctor
        [Fact]
        public void LocalProjectCtor_ThrowsArgumentNullException_WhenNullRootFolderIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalProject(new MockFileSystem(), null));
        }

        [Fact]
        public void LocalProjectCtor_ThrowsArgumentNullException_WhenNullFileSystemIsProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalProject(null, "SomeFolderHere"));
        }

        [Fact]
        public void LocalProjectCtor_ThrowsException_WhenRootFolderDoesNotExist()
        {
            Assert.Throws<Exception>(() => new LocalProject(new MockFileSystem(), "SomeRandomFolder"));
        }

        [Fact]
        public void LocalProjectCtor_DoesNotThrowException_WhenFileSystemAndRootFolderExists()
        {
            var rootFolder = "SomeRootHere";
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootFolder);

            new LocalProject(new MockFileSystem(), rootFolder);
        }
        #endregion
    }
}