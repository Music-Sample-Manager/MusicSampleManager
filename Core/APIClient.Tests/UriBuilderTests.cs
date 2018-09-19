using System;
using Xunit;

namespace APIClient.Tests
{
    public class UriBuilderTests
    {
        #region BuildUri
        [Fact]
        public void BuildUri_ShouldNotThrowException()
        {
            UriBuilder.BuildUri("https://www.example.com", string.Empty, string.Empty);
        }

        [Fact]
        public void BuildUri_ThrowsArgumentNullException_WhenBaseUrlIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => UriBuilder.BuildUri(null, string.Empty, string.Empty));
        }

        [Fact]
        public void BuildUri_ThrowsArgumentException_WhenBaseUrlIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => UriBuilder.BuildUri(string.Empty, string.Empty, string.Empty));
        }

        [Fact]
        public void BuildUri_ReturnsCorrectlyFormattedUri_WhenInputIsValid()
        {
            string baseUrl = "https://example.com";
            string path = "api/someMethod";
            string urlParameter = "someKey=someValue";

            var result = UriBuilder.BuildUri(baseUrl, path, urlParameter);

            Assert.Equal($"{baseUrl}//{path}?{urlParameter}", result.ToString());
        }
        #endregion
    }
}
