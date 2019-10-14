using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Services
{
public class AzureBlobFileClient : IFileClient
    {
        private CloudBlobClient _blobClient;

        public AzureBlobFileClient(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            _blobClient = account.CreateCloudBlobClient();
        }

        public Task DeleteFile(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return blob.DeleteIfExistsAsync();
        }

        public Task<bool> FileExists(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return blob.ExistsAsync();
        }

        public async Task<Stream> GetFile(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            var mem = new MemoryStream();
            await blob.DownloadToStreamAsync(mem).ConfigureAwait(false);
            mem.Seek(0, SeekOrigin.Begin);

            return mem;
        }

        public async Task<string> GetFileUrl(string storeName, string filePath)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());
            string url = null;

            if (await blob.ExistsAsync().ConfigureAwait(false))
            {
                url = blob.Uri.AbsoluteUri;
            }

            return url;
        }

        public Task SaveFile(string storeName, string filePath, Stream fileStream)
        {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return blob.UploadFromStreamAsync(fileStream);
        }
    }
}
