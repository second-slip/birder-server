//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.File;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace Birder.Services
//{
//    public class AzureFileShareClient : IFileClient
//    {
//        private CloudFileClient _fileClient;

//        public AzureFileShareClient(string connectionString)
//        {
//            var account = CloudStorageAccount.Parse(connectionString);
//            _fileClient = account.CreateCloudFileClient();
//        }

//        public async Task DeleteFile(string storeName, string filePath)
//        {
//            var share = _fileClient.GetShareReference(storeName);
//            var folder = share.GetRootDirectoryReference();
//            var pathParts = filePath.Split('/');
//            var fileName = pathParts[pathParts.Length - 1];

//            for (var i = 0; i < pathParts.Length - 2; i++)
//            {
//                folder = folder.GetDirectoryReference(pathParts[i]);
//                if (!await folder.ExistsAsync().ConfigureAwait(false))
//                {
//                    return;
//                }
//            }

//            var fileRef = folder.GetFileReference(fileName);

//            await fileRef.DeleteIfExistsAsync().ConfigureAwait(false);
//        }

//        public async Task<bool> FileExists(string storeName, string filePath)
//        {
//            var share = _fileClient.GetShareReference(storeName);
//            var folder = share.GetRootDirectoryReference();
//            var pathParts = filePath.Split('/');
//            var fileName = pathParts[pathParts.Length - 1];

//            for (var i = 0; i < pathParts.Length - 2; i++)
//            {
//                folder = folder.GetDirectoryReference(pathParts[i]);
//                if (!await folder.ExistsAsync().ConfigureAwait(false))
//                {
//                    return false;
//                }
//            }

//            var fileRef = folder.GetFileReference(fileName);

//            return await fileRef.ExistsAsync().ConfigureAwait(false);
//        }

//        public Task<List<string>> GetAllFileUrl(string storeName)
//        {
//            throw new System.NotImplementedException();
//        }

//        public async Task<Stream> GetFile(string storeName, string filePath)
//        {
//            var share = _fileClient.GetShareReference(storeName);
//            var folder = share.GetRootDirectoryReference();
//            var pathParts = filePath.Split('/');
//            var fileName = pathParts[pathParts.Length - 1];

//            for (var i = 0; i < pathParts.Length - 2; i++)
//            {
//                folder = folder.GetDirectoryReference(pathParts[i]);
//                if (!await folder.ExistsAsync().ConfigureAwait(false))
//                {
//                    return null;
//                }
//            }

//            var fileRef = folder.GetFileReference(fileName);
//            if (!await fileRef.ExistsAsync().ConfigureAwait(false))
//            {
//                return null;
//            }

//            return await fileRef.OpenReadAsync().ConfigureAwait(false);
//        }

//        public Task<string> GetFileUrl(string storeName, string filePath)
//        {
//            return Task.FromResult((string)null);
//        }

//        public async Task SaveFile(string storeName, string filePath, Stream fileStream)
//        {
//            var share = _fileClient.GetShareReference(storeName);
//            var folder = share.GetRootDirectoryReference();
//            var pathParts = filePath.Split('/');
//            var fileName = pathParts[pathParts.Length - 1];

//            for (var i = 0; i < pathParts.Length - 2; i++)
//            {
//                folder = folder.GetDirectoryReference(pathParts[i]);

//                await folder.CreateIfNotExistsAsync().ConfigureAwait(false);
//            }

//            var fileRef = folder.GetFileReference(fileName);

//            await fileRef.UploadFromStreamAsync(fileStream).ConfigureAwait(false);
//        }
//    }
//}
