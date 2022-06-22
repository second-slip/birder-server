using Newtonsoft.Json;

namespace Birder.Templates;

public class ConfirmEmailDto
{
    public string Email { get; set; }

    [JsonProperty("username")] public string Username { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }
}
