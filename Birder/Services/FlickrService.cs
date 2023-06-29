using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Birder.Infrastructure.CustomExceptions;

namespace Birder.Services;

public interface IFlickrService
{
    Task<string> GetThumbnailUrl(string queryString);
}

public class FlickrService : IFlickrService
{
    private readonly IHttpClientFactory _httpFactory;
    private FlickrOptions _options { get; }

    public FlickrService(IOptions<FlickrOptions> optionsAccessor, IHttpClientFactory httpFactory)
    {
        _options = optionsAccessor.Value;
        _httpFactory = httpFactory;
    }

    public async Task<string> GetThumbnailUrl(string queryString)
    {
        if (string.IsNullOrEmpty(queryString))
            throw new ArgumentException("The argument is null or empty", nameof(queryString));

        var encodedQuery = EncodeQueryParameter(queryString);
        var url = BuildUrl(encodedQuery);

        var client = _httpFactory.CreateClient("FlickrApiClient");
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonOpts = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNameCaseInsensitive = true };
            var contentStream = await response.Content.ReadAsStreamAsync();
            var flickrResponse = await JsonSerializer.DeserializeAsync<FlickrResponse>(contentStream, jsonOpts);

            return flickrResponse.Photo.PhotoDetail.FirstOrDefault().Url;
        }
        else
        {
            // can deserialise the error respose object FlickrExceptionResponse, but I haven't...
            throw new FlickrException(response.StatusCode, "Error response from FlickrApi: " + response.ReasonPhrase);
        }
    }

    private string EncodeQueryParameter(string queryString)
    {
        return UrlEncoder.Default.Encode(queryString);
    }

    private string BuildUrl(string encodedQuery)
    {
        var url = $"https://api.flickr.com/services/rest/?api_key={_options.FlickrApiKey}&format=json&nojsoncallback=1&method=flickr.photos.search&per_page=1&page=1&extras=url_q&content_type=1&text={encodedQuery}";
        return url;
    }
}