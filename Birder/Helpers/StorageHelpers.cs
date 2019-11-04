using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Birder.Helpers
{
    public static class StorageHelpers
    {
        public static string GetFileName(IFormFile file)
        {
            var fileExt = Path.GetExtension(file.FileName);
            return string.Concat(Guid.NewGuid(), fileExt);
        }
        public static List<PhotographDto> UpdatePhotographsDto(List<string> urls)
        {
            var model = new List<PhotographDto>();
            foreach (var url in urls)
            {
                model.Add(new PhotographDto()
                {
                    Address = url,
                    Filename = Path.GetFileName(new Uri(url).AbsolutePath)

                });
            }
            return model;
        }

        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}
