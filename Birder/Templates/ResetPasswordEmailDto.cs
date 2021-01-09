using Newtonsoft.Json;
using System;

namespace Birder.Templates
{
    public class ResetPasswordEmailDto
    {
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}
