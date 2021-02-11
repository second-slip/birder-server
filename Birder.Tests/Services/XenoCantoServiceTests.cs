using Birder.Infrastructure.CustomExceptions;
using Birder.Services;
using Birder.ViewModels;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Services
{
    public class XenoCantoServiceTests
    {
        [Fact]
        public async Task Returns_A_Recording()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.OkResponse);
            var sut = new XenoCantoService(clientFactory);

            var result = await sut.GetSpeciesRecordings("Branta canadensis");

            Assert.IsType<List<RecordingViewModel>>(result);
        }

        [Fact]
        public async Task Returns_Expected_Values_From_the_Api()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.OkResponse);
            var sut = new XenoCantoService(clientFactory);

            var result = await sut.GetSpeciesRecordings("Branta canadensis");

            Assert.Equal("//a/b/c/d/testFileName.mp3", result[0].Url);
            Assert.Equal(0, result[0].Id);
        }

        [Fact]
        public async Task Returns_XenoCantoException_When_Called_With_Bad_Argument()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.NotFoundResponse,
                HttpStatusCode.NotFound);
            var sut = new XenoCantoService(clientFactory);

            var result = await Assert.ThrowsAsync<XenoCantoException>(() => sut.GetSpeciesRecordings("dnifihurvbfvije"));
            Assert.Equal(404, (int)result.StatusCode);
        }

        [Fact]
        public async Task Returns_XenoCantoException_On_XenoCantoInternalError()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.InternalErrorResponse,
                HttpStatusCode.InternalServerError);
            var sut = new XenoCantoService(clientFactory);

            var result = await Assert.ThrowsAsync<XenoCantoException>(() => sut.GetSpeciesRecordings("Branta canadensis"));
            Assert.Equal(500, (int)result.StatusCode);
        }
    }






    public static class ClientBuilder
    {
        public static IHttpClientFactory XenoCantoClientFactory(StringContent content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
            var client = new HttpClient(handler.Object);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(client);
            return clientFactory.Object;
        }
    }

    public static class XenoCantoResponses
    {
        public static StringContent OkResponse => BuildOkResponse();
        public static StringContent UnauthorizedResponse => BuildUnauthorizedResponse();
        public static StringContent NotFoundResponse => BuildNotFoundResponse();
        public static StringContent InternalErrorResponse => BuildInternalErrorResponse();

        private static StringContent BuildOkResponse()
        {
            var response = new XenoCantoResponse
            {
                Recordings = new List<Recording>
                {
                    new Recording{ FileName = "testFileName.mp3", Sono = new Sono { Small = "//a/b/c/d/e/f/g/", Med = "testMedSono", Large = "testLargeSono", Full = "testFullsono" } }
                }
            };
            var json = JsonSerializer.Serialize(response);
            return new StringContent(json);
        }

        private static StringContent BuildUnauthorizedResponse()
        {
            var json = JsonSerializer.Serialize(new { Cod = 401, Message = "Invalid Api key." });
            return new StringContent(json);
        }

        private static StringContent BuildNotFoundResponse()
        {
            var json = JsonSerializer.Serialize(new { Cod = 404, Message = "city not found" });
            return new StringContent(json);
        }

        private static StringContent BuildInternalErrorResponse()
        {
            var json = JsonSerializer.Serialize(new { Cod = 500, Message = "Internal Error." });
            return new StringContent(json);
        }
    }
}
