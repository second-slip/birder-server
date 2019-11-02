using System;
using System.Collections.Generic;
using System.IO;
using static Birder.Controllers.PhotographController;

namespace Birder.Helpers
{
    public static class StorageHelpers
    {
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
    }
}
