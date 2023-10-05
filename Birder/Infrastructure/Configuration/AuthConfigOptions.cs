namespace Birder.Infrastructure.Configuration;

public class ConfigOptions
{
    public const string Config = "Config";
    public string BaseUrl { get; set; }
    public string TokenKey { get; set; }
    public string DevMail { get; set; }
    public string SendGridUser { get; set; }
    public string SendGridKey { get; set; }
    public string SendGridMail { get; set; }
}