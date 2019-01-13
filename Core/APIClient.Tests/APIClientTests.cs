using Domain;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace APIClient.Tests
{
    public class APIClientTests
    {
        #region FindPackageByNameTests
        [Fact]
        public void TryFindPackageByName_ReturnsPackageAndOutParamReturnsTrue_WhenPackageExists()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(new Package("TestPackageName")))
               })
               .Verifiable();
            var sut = new APIClient("https://example.com", new HttpClient(handlerMock.Object));

            var searchSucceeded = sut.TryFindPackageByName("TestPackageName", out Package package);

            Assert.True(searchSucceeded);
            Assert.NotNull(package);
            Assert.Equal("TestPackageName", package.Identifier);
        }

        [Fact]
        public void TryFindPackageByName_ReturnsNullAndOutParamReturnsTrue_WhenPackageDoesNotExist()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(null))
               })
               .Verifiable();
            var sut = new APIClient("https://example.com", new HttpClient(handlerMock.Object));

            var searchSucceeded = sut.TryFindPackageByName("TestPackageName", out Package package);

            Assert.True(searchSucceeded);
            Assert.Null(package);
        }

        [Fact]
        public void TryFindPackageByName_OutParamReturnsFalse_WhenAPIIsNotAvailable()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.InternalServerError
               })
               .Verifiable();
            var sut = new APIClient("https://example.com", new HttpClient(handlerMock.Object));

            var searchSucceeded = sut.TryFindPackageByName("TestPackageName", out Package package);

            Assert.False(searchSucceeded);
            Assert.Null(package);
        }
        #endregion
    }
}