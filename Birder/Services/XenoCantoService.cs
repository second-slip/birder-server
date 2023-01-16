using Birder.Infrastructure.CustomExceptions;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Birder.Services;
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
        if (string.IsNullOrEmpty(species))
            throw new ArgumentException("The argument is null or empty", nameof(species));

        string formattedSearchTerm = XenoCantoServiceHelpers.FormatSearchTerm(species);
        string url = XenoCantoServiceHelpers.BuildXenoCantoApiUrl(formattedSearchTerm);
        var recordings = new List<RecordingViewModel>();

        var client = _httpFactory.CreateClient("XenoCantoClient");
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonOpts = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNameCaseInsensitive = true };
            var contentStream = await response.Content.ReadAsStreamAsync();
            var xenoCantoResponse = await JsonSerializer.DeserializeAsync<XenoCantoResponse>(contentStream, jsonOpts);

            int index = 0;
            foreach (var forecast in xenoCantoResponse.Recordings)
            {
                recordings.Add(new RecordingViewModel
                {
                    Id = index,
                    Url = XenoCantoServiceHelpers.BuildRecordingUrl(forecast.Sono.Small, forecast.FileName),
                });

                index++;
            }

            return recordings;
        }
        else
        {
            // can deserialise the error respose object XenoCantoErrorResponse, but I haven't...
            throw new XenoCantoException(response.StatusCode, "Error response from XenoCantoApi: " + response.ReasonPhrase);
        }
    }
}