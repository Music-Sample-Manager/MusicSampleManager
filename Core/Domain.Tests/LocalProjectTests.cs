using System;
using System.IO.Abstractions.TestingHelpers;
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
    }
}