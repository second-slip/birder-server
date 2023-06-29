using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

using System.Web;

namespace Birder.Services;

public interface IUrlService
{
    Uri GetResetPasswordUrl(string code);
    Uri GetConfirmEmailUrl(string username, string code);
}

public class UrlService : IUrlService
{
    public UrlService(IOptions<ConfigOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    public ConfigOptions Options { get; }

    public Uri GetConfirmEmailUrl(string username, string code)
    {
        var queryParams = new Dictionary<string, string>()
                {
                    {"username", username },
                    {"code", code }
                };

        var url = string.Concat(Options.BaseUrl, "/api/Account/ConfirmEmail");
        return new Uri(QueryHelpers.AddQueryString(url, queryParams));
    }

    public Uri GetResetPasswordUrl(string code)
    {
        var url = string.Concat(Options.BaseUrl, "/reset-password/", HttpUtility.UrlEncode(code));
        return new Uri(url);
    }
}
