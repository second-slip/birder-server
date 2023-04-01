using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using Birder.Infrastructure.CustomExceptions;
using Microsoft.Extensions.Options;
using Moq.Protected;

namespace Birder.Tests.Services;

public class FlickrServiceTests
{

    IOptions<FlickrOptions> testOptions = Options.Create<FlickrOptions>(new FlickrOptions()
    { FlickrApiKey = "key", FlickrSecret = "secret" });


    [Fact]
    public async Task Returns_A_FlickrResponse_With_Expected_values()
    {
        var clientFactory = ClientBuilder.FlickrClientFactory(FlickrResponses.OkResponse);
        var service = new FlickrService(testOptions, clientFactory);

        var result = await service.GetThumbnailUrl("Branta canadensis");

        Assert.IsType<string>(result);
        Assert.Equal("https://www.hello.com", result);
    }

    [Fact]
    public async Task Returns_ArgumentException_When_Argument_Is_Null_Or_Empty()
    {
        var clientFactory = ClientBuilder.FlickrClientFactory(FlickrResponses.OkResponse);
        var service = new FlickrService(testOptions, clientFactory);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetThumbnailUrl(null));
        Assert.Equal("The argument is null or empty (Parameter 'queryString')", ex.Message);
    }

    [Fact]
    public async Task Returns_FlickrException_When_Called_With_Bad_Argument()
    {
        var clientFactory = ClientBuilder.FlickrClientFactory(FlickrResponses.NotFoundResponse,
            HttpStatusCode.NotFound);
        var service = new FlickrService(testOptions, clientFactory);

        var result = await Assert.ThrowsAsync<FlickrException>(() => service.GetThumbnailUrl("dnifihurvbfvije"));
        Assert.Equal(404, (int)result.StatusCode);
    }

    [Fact]
    public async Task Returns_FlickrException_On_FlickrInternalError()
    {
        var clientFactory = ClientBuilder.FlickrClientFactory(FlickrResponses.InternalErrorResponse,
            HttpStatusCode.InternalServerError);
        var service = new FlickrService(testOptions, clientFactory);

        var result = await Assert.ThrowsAsync<FlickrException>(() => service.GetThumbnailUrl("Branta canadensis"));
        Assert.Equal(500, (int)result.StatusCode);
    }


    public static class ClientBuilder
    {
        public static IHttpClientFactory FlickrClientFactory(StringContent content, HttpStatusCode statusCode = HttpStatusCode.OK)
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

    public static class FlickrResponses
    {
        public static StringContent OkResponse => BuildOkResponse();
        public static StringContent UnauthorizedResponse => BuildUnauthorizedResponse();
        public static StringContent NotFoundResponse => BuildNotFoundResponse();
        public static StringContent InternalErrorResponse => BuildInternalErrorResponse();

        private static StringContent BuildOkResponse()
        {
            var response = new FlickrResponse
            {
                Photo = new Photo
                {
                    Page = 1,
                    PhotoDetail = new List<PhotoDetail>() { new PhotoDetail { Id = "1", Url = "https://www.hello.com" } }
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