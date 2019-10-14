using System.IO;
using System.Threading.Tasks;

namespace Birder.Services
{
    public class LocalFileClient : IFileClient
    {
        private string _fileRoot;

        public LocalFileClient(string fileRoot)
        {
            _fileRoot = fileRoot;
        }

        public Task DeleteFile(string storeName, string filePath)
        {
            var path = Path.Combine(_fileRoot, storeName, filePath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return Task.CompletedTask;
        }

        public Task<bool> FileExists(string storeName, string filePath)
        {
            var path = Path.Combine(_fileRoot, storeName, filePath);

            return Task.FromResult(File.Exists(path));
        }

        public Task<Stream> GetFile(string storeName, string filePath)
        {
            var path = Path.Combine(_fileRoot, storeName, filePath);
            Stream stream = null;

            if (File.Exists(path))
            {
                stream = File.OpenRead(path);
            }

            return Task.FromResult(stream);
        }

        public Task<string> GetFileUrl(string storeName, string filePath)
        {
            return Task.FromResult((string)null);
        }

        public async Task<string> SaveFile(string storeName, string filePath, Stream fileStream)
        {
            var path = Path.Combine(_fileRoot, storeName, filePath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var file = new FileStream(path, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(file).ConfigureAwait(false);
            }

            return await Task.FromResult(path);
        }
    }
}
    
