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
            Assert.Throws<ArgumentNullException>(() => new Package(null));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentException_WhenIdentifierIsEmptyString()
        {
            Assert.Throws<ArgumentException>(() => new Package(string.Empty));
        }
        #endregion
    }
}