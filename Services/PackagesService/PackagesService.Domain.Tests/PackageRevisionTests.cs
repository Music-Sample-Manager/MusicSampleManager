using Semver;
using System;
using System.IO.Compression;
using Xunit;

namespace PackagesService.Domain.Tests
{
    public class PackageRevisionTests
    {
        #region Ctor
        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenPackageParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(null, new SemVersion(0), new ZipArchive(null)));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenVersionNumberParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(new Package("Test.Package"), null, new ZipArchive(null)));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenContentsParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(new Package("Test.Package"), new SemVersion(0), null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenAllParametersAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(null, null, null));
        }
        #endregion
    }
}
