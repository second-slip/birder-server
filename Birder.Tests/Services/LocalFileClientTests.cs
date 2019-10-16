using Birder.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Services
{
    public class LocalFileClientTests
    {
        private readonly IFileClient _fileClient;

        public LocalFileClientTests()
        {
            _fileClient = new LocalFileClient(@"C:\Users\Andre\OneDrive");
        }

        [Fact]
        public async Task x_y_z()
        {


            string a = "https://birderstorage.blob.core.windows.net/profile/birder.png";
            a.Replace("birder.png", "test");

            var t = 4;

            

            //var a = await _fileClient.GetFile("Desktop", "BirderDictionaryDefinitionExample.png");

            //await _fileClient.SaveFile("Birder", "Test.png", a);

            //var fileName = Sanitize("/" + UserId + "/" + file.FileName);

            //using (var fileStream = file.OpenReadStream())
            //{
            //    await _fileClient.SaveFile("Contracts", fileName, fileStream);
            //}

            Assert.NotNull(a);

        }
    }
}
