using System.IO;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IFileClient
    {
        Task DeleteFile(string storeName, string filePath);
        Task<bool> FileExists(string storeName, string filePath);
        Task<Stream> GetFile(string storeName, string filePath);
        Task<string> GetFileUrl(string storeName, string filePath);
        Task SaveFile(string storeName, string filePath, Stream fileStream);
    }
}
