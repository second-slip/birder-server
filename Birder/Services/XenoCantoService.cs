using Birder.Helpers;
using Birder.Infrastructure.CustomExceptions;
using Birder.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IXenoCantoService
    {
        Task<List<RecordingViewModel>> GetSpeciesRecordings(string species);
    }

    public class XenoCantoService : IXenoCantoService
    {
        private readonly IHttpClientFactory _httpFactory;

        public XenoCantoService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<List<RecordingViewModel>> GetSpeciesRecordings(string species)
        {
            string formattedSearchTerm = XenoCantoServiceHelpers.FormatSearchTerm(species);
            string url = XenoCantoServiceHelpers.BuildXenoCantoApiUrl(formattedSearchTerm);
            var forecasts = new List<RecordingViewModel>();

            var client = _httpFactory.CreateClient("XenoCantoClient");
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonOpts = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };
                var contentStream = await response.Content.ReadAsStreamAsync();
                var openWeatherResponse = await JsonSerializer.DeserializeAsync<XenoCantoResponse>(contentStream, jsonOpts);
                
                int index = 0;
                foreach (var forecast in openWeatherResponse.Recordings)
                {
                    forecasts.Add(new RecordingViewModel
                    {
                        Id = index,
                        Url = XenoCantoServiceHelpers.BuildRecordingUrl(forecast.Sono.Small, forecast.FileName),
                    });

                    index++;
                }

                return forecasts;
            }
            else
            {
                // can deserialise the error respose object XenoCantoErrorResponse, but I haven't...
                throw new XenoCantoException(response.StatusCode, "Error response from XenoCantoApi: " + response.ReasonPhrase);
            }
        }
    }
}
