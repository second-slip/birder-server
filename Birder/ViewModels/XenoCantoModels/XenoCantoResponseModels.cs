
using System.Text.Json.Serialization;

namespace Birder.ViewModels;

public class XenoCantoResponse
{
    [JsonPropertyName("numRecordings")]
    public string Qty { get; set; }

    [JsonPropertyName("recordings")]
    public List<Recording> Recordings { get; set; }
}

public class Recording
{
    //[JsonPropertyName("url")]
    //public string Url { get; set; }

    [JsonPropertyName("file-name")]
    public string FileName { get; set; }

    [JsonPropertyName("sono")]
    public Sono Sono { get; set; }
}

public class Sono
{
    [JsonPropertyName("small")]
    public string Small { get; set; }

    [JsonPropertyName("med")]
    public string Med { get; set; }

    [JsonPropertyName("large")]
    public string Large { get; set; }

    [JsonPropertyName("full")]
    public string Full { get; set; }
}

public class XenoCantoErrorResponse
{
    [JsonPropertyName("code")] public string Code { get; }

    [JsonPropertyName("message")] public string Message { get; }
}