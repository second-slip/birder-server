namespace Birder.ViewModels;

public class AuthConfigOptions
{
    public const string AuthConfig = "AuthConfig";
    public string BaseUrl { get; set; }
    public string TokenKey { get; set; }
}