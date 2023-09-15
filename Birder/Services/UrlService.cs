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

    private ConfigOptions Options { get; }

    public Uri GetConfirmEmailUrl(string username, string code)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("method argument is not valid", nameof(username));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("method argument is not valid", nameof(code));

        var queryParams = new Dictionary<string, string>()
                {
                    {"username", username },
                    {"code", code }
                };

        return new Uri(QueryHelpers.AddQueryString(CreateUrl(), queryParams));
    }

    private string CreateUrl()
    {
        return string.Concat(Options.BaseUrl, "/api/Account/ConfirmEmail");
    }

    public Uri GetResetPasswordUrl(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("method argument is not valid", nameof(code));

        var url = string.Concat(Options.BaseUrl, "/reset-password/", HttpUtility.UrlEncode(code));
        return new Uri(url);
    }
}