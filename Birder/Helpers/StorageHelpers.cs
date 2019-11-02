using System;
using System.Collections.Generic;
using System.IO;
using static Birder.Controllers.PhotographController;

namespace Birder.Helpers
{
    public static class StorageHelpers
    {
        public static List<PhotographsDto> UpdatePhotographsDto(List<string> urls)
        {
            var model = new List<PhotographsDto>();
            foreach (var url in urls)
            {
                model.Add(new PhotographsDto()
                {
                    Address = url,
                    Filename = Path.GetFileName(new Uri(url).AbsolutePath)

                });
            }
            return model;
        }
    }
}
