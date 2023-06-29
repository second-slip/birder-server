namespace Birder.Infrastructure.Configuration;

public class ConfigOptions
{
    public const string Config = "Config";
    public string BaseUrl { get; set; }
    public string TokenKey { get; set; }
    public string DevMail { get; set; }
}