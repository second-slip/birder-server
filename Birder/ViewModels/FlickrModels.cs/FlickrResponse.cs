using System.Text.Json.Serialization;


namespace Birder.ViewModels;

public class FlickrErrorResponse
{
    [JsonPropertyName("code")] public string Code { get; }

    [JsonPropertyName("message")] public string Message { get; }
}

public class FlickrResponse
{
    [JsonPropertyName("photos")]
    public Photo Photo { get; set; }
}

public class Photo
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("photo")]
    public List<PhotoDetail> PhotoDetail { get; set; }
}

public class PhotoDetail
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("url_q")]
    public string Url { get; set; }
}