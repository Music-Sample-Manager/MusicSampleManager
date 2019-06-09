using System;
using Xunit;

namespace PackagesService.Domain.Tests
{
    public class PackageTests
    {
        #region Ctor
        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_WhenIdentifierIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Package(0, null, string.Empty, 0));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentException_WhenIdentifierIsEmptyString()
        {
            Assert.Throws<ArgumentException>(() => new Package(0, string.Empty, string.Empty, 0));
        }
        #endregion
    }
}