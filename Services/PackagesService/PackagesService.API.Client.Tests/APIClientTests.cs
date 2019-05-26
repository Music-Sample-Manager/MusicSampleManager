using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PackagesService.Domain;
using Semver;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PackagesService.API.Client.Tests
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

        #region GetLatestPackageZipTests
        [Fact]
        public async void GetLatestPackageZipTests_ThrowsArgumentException_WhenTargetPackageNameIsEmpty()
        {
            var sut = new APIClient("https://example.com", new HttpClient());

            var exception = Assert.ThrowsAsync<ArgumentException>(() => sut.GetLatestPackageZip(string.Empty));

            Assert.NotNull(exception);
        }

        [Fact]
        public async void GetLatestPackageZipTests_ThrowsArgumentNullException_WhenTargetPackageNameIsNull()
        {
            var sut = new APIClient("https://example.com", new HttpClient());

            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetLatestPackageZip(null));

            Assert.NotNull(exception);
        }

        [Fact]
        public async void GetLatestPackageZipTests_Succeeds_WhenTargetPackageNameIsValid()
        {
            var responseObject = new PackageRevision(new Package("test"),
                                                     new SemVersion(new Version("1.2.3.4")),
                                                     new ZipArchive(new MemoryStream(Properties.Resources.mockZip)));
            string jsonContent = JsonConvert.SerializeObject(responseObject,
                                                             Formatting.Indented,
                                                             new JsonSerializerSettings
                                                             {
                                                                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                             });

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent)
            };
            var mockHttpHandler = new Mock<HttpClientHandler>();
            mockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(mockResponse));
            var mockHttpClient = new HttpClient(mockHttpHandler.Object);
            var sut = new APIClient("https://example.com", mockHttpClient);

            var result = await sut.GetLatestPackageZip("SomeValidPackageName");

            Assert.NotNull(result);
            Assert.IsType<PackageRevision>(result);
        }
        #endregion
    }
}