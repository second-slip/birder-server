using Birder.Controllers;
using Birder.Services;
using Birder.Tests.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class RecordingControllerTests
    {
        [Fact]
        public async Task Returns_OkResult_With_Recording()
        {

            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.OkResponse);
            var service = new XenoCantoService(clientFactory);
            var sut = new RecordingController(new NullLogger<RecordingController>(), service);

            var result = await sut.GetRecordingsAsync("Branta canadensis") as OkObjectResult;

            Assert.IsType<List<RecordingViewModel>>(result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Returns_400_Result_When_Missing_Location()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.NotFoundResponse);
            var service = new XenoCantoService(clientFactory);
            var sut = new RecordingController(new NullLogger<RecordingController>(), service);

            var result = await sut.GetRecordingsAsync(String.Empty) as ObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Returns_BadRequestResult_When_Location_Not_Found()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.NotFoundResponse,
                HttpStatusCode.NotFound);
            var service = new XenoCantoService(clientFactory);
            var sut = new RecordingController(new NullLogger<RecordingController>(), service);

            var result = await sut.GetRecordingsAsync("Westworld") as ObjectResult;

            Assert.Contains("not found", result.Value.ToString());
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Returns_500_When_Api_Returns_Error()
        {
            var clientFactory = ClientBuilder.XenoCantoClientFactory(XenoCantoResponses.UnauthorizedResponse,
                HttpStatusCode.Unauthorized);
            var service = new XenoCantoService(clientFactory);
            var sut = new RecordingController(new NullLogger<RecordingController>(), service);

            var result = await sut.GetRecordingsAsync("Rio de Janeiro") as ObjectResult;

            Assert.Contains("Error response from XenoCantoApi: Unauthorized", result.Value.ToString());
            Assert.Equal(500, result.StatusCode);
        }
    }
}
