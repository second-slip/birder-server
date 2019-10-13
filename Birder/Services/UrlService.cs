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

            return new Uri(QueryHelpers.AddQueryString(_configuration["Url:ConfirmEmailUrl"], queryParams));
        }

        public Uri GetResetPasswordUrl(string code)
        {
            return new Uri(String.Concat(_configuration["Url:ResetPasswordUrl"], HttpUtility.UrlEncode(code)));
        }
    }
}
