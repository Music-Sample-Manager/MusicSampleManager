using PublicWebsite;
using PublicWebsite.APIClient;
using System;
using Xunit;

namespace API.Tests
{
    public class PackagesControllerTests
    {
        private readonly APIClient client = new APIClient();


        [Fact]
        public void Get_WithEmptyPackageName_Throws()
        {
            var sut = new PackagesController(client);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => sut.Get(string.Empty));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Get_WithNullPackageName_Throws()
        {
            var sut = new PackagesController(client);

            Assert.Throws<ArgumentNullException>(() => sut.Get(null));
        }
    }
}