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
            string url = BuildXenoCantoApiUrl(species);
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
                        Url = BuildRecordingUrl(forecast.Sono.Small, forecast.FileName),
                    });

                    index++;
                }

                return forecasts;
            }
            else
            {
                // can deserialise the error respose object XenoCantoErrorResponse, but I haven't...
                throw new XenoCantoException(response.StatusCode, "Error response from XentoCantoApi: " + response.ReasonPhrase);
            }
        }

        private string BuildRecordingUrl(string baseUrl, string fileName)
        {
            var substring = baseUrl.Substring(0, IndexOfNth(baseUrl, '/', 6) + 1);
            return string.Concat(substring, fileName);
        }

        private int IndexOfNth(string str, char c, int n)
        {
            int remaining = n;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                {
                    remaining--;
                    if (remaining == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private string BuildXenoCantoApiUrl(string species)
        {
            string searchSpeciesFormatted = species.Replace(" ", "+");

            return $"https://www.xeno-canto.org/api/2/recordings?query=" +
                   $"{searchSpeciesFormatted}" +
                   $"+len_gt:40";
        }
    }
}
