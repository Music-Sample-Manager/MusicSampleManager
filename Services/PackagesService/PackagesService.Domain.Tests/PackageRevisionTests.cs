using Semver;
using System;
using System.IO;
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
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(new Package("Test.Package", string.Empty, 0), null, new ZipArchive(null)));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenContentsParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(new Package("Test.Package", string.Empty, 0), new SemVersion(0), null));
        }

        [Fact]
        public void Ctor_ThrowsArgumentNullException_WhenAllParametersAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageRevision(null, null, null));
        }

        [Fact]
        public void Ctor_Succeeds_WhenAllParametersAreValid()
        {
            var sut = new PackageRevision(new Package("Test.Package", string.Empty, 0), new SemVersion(0), new ZipArchive(new MemoryStream(Properties.Resources.mockZip)));

            Assert.NotNull(sut);
        }
        #endregion

        #region ContentsUri
        [Fact]
        public void ContentsUri_ReturnsCorrectValue_WhenPackageRevisionObjectIsValid()
        {
            var sut = new PackageRevision(new Package("Test.Package", string.Empty, 0),
                                          new SemVersion(1, 2, 3),
                                          new ZipArchive(new MemoryStream(Properties.Resources.mockZip)));

            var uri = sut.ContentsUri();

            Assert.NotNull(sut);
            Assert.Equal("https://msmpackagedbsa.blob.core.windows.net/all-packages/Test.Package/1.2.3.sf2",
                         uri.AbsoluteUri);
        }

        [Fact]
        public void LogicToRedirectRequestsToBlobStorage_IsImplemented()
        {
            throw new NotImplementedException("Need to do this! So that the system actually downloads the soundfont file.");
        }

        [Fact]
        public void ContentsUri_ShouldBeMovedToDAL()
        {
            throw new NotImplementedException("The file URI is a detail. Need to get it out of the domain layer.");
        }

        [Fact]
        public void AlsoInTheMiddleOfRefactoringToCleanArchitecture()
        {
            throw new NotImplementedException("Currently working on converting 'CreatePackage' into a fully Clean spike of functionality. The dependencies should all point in the correct direction there now. Just need to finish wiring it up.");
        }
        #endregion
    }
}
