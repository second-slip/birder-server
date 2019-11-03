using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
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

            // ASC: adjustment
            if (await blob.ExistsAsync().ConfigureAwait(false))
            {
                var mem = new MemoryStream();
                await blob.DownloadToStreamAsync(mem).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);

                return mem;
            }

            //ASC: adjustment
            return await Task.FromResult<Stream>(null);
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


        public async Task<List<string>> GetAllFileUrl(string storeName)
        {
            // List the blobs in the container.
            //Console.WriteLine("List blobs in container.");
            var container = _blobClient.GetContainerReference(storeName);
            //var blob = container.GetBlockBlobReference(filePath.ToLower());

            var urls = new List<string>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    urls.Add(item.Uri.ToString());
                }
            } while (blobContinuationToken != null); // Loop while the continuation token is not null.

            return urls;
        }

        public Task SaveFile(string storeName, string filePath, Stream fileStream)
        {
            //Todo: create container...

            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return blob.UploadFromStreamAsync(fileStream);
        }
    }
}
