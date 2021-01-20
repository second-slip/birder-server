using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Web;

namespace Birder.Services
{
    public interface IUrlService
    {
        Uri GetResetPasswordUrl(string code);
        Uri GetConfirmEmailUrl(string username, string code);
    }

    public class UrlService : IUrlService
    {
        private readonly IConfiguration _configuration;

        public UrlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Uri GetConfirmEmailUrl(string username, string code)
        {
            var queryParams = new Dictionary<string, string>()
                {
                    {"username", username },
                    {"code", code }
                };

            var url = string.Concat(_configuration["Scheme"], _configuration["Domain"], "/api/Account/ConfirmEmail");

            return new Uri(QueryHelpers.AddQueryString(url, queryParams));
        }

        public Uri GetResetPasswordUrl(string code)
        {
            var url = string.Concat(_configuration["Scheme"], _configuration["Domain"], "/reset-password/", HttpUtility.UrlEncode(code));

            return new Uri(url);
        }
    }
}
