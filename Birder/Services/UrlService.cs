using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Web;

namespace Birder.Services
{
    public interface IUrlService
    {
        Uri ResetPasswordUrl(string code);
        Uri ConfirmEmailUrl(string username, string code);

    }
    public class UrlService : IUrlService
    {
        private readonly IConfiguration _config;

        public UrlService(IConfiguration config)
        {
            _config = config;
        }

        public Uri ConfirmEmailUrl(string username, string code)
        {
            var queryParams = new Dictionary<string, string>()
                {
                    {"username", username },
                    {"code", code }
                };

            return new Uri(QueryHelpers.AddQueryString(_config["Url:ConfirmEmailUrl"], queryParams));
        }

        public Uri ResetPasswordUrl(string code)
        {
            return new Uri(String.Concat(_config["Url:ResetPasswordUrl"], HttpUtility.UrlEncode(code)));
        }
    }

    //var y = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

    //UriBuilder uriBuilder = new UriBuilder();
    //uriBuilder.Scheme = this.Request.Scheme;
    //    uriBuilder.Host = this.Request.Host.Host;
    //    uriBuilder.Path = this.Request.PathBase;
    //    Uri uri = uriBuilder.Uri;
    //returns /api/product/list?foo=bar
    //string url = QueryHelpers.AddQueryString(k, "code", code);

    //Multiple Parameters
    //var queryParams = new Dictionary<string, string>()
    //    {
    //        {"cat", "221" },
    //        {"gender", "boy" },
    //        {"age","4,5,6" }
    //    };
    //returns /api/product/list?cat=221&gender=boy&age=4,5,6
    //url = QueryHelpers.AddQueryString("/api/product/list", queryParams);
}
