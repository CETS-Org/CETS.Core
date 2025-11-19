using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.Common.Storage
{
    public interface IFileStorageService
    {
        Task DeleteFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);

        Task<(string PresignedUrl, string FilePath)> GetPresignedPutUrlAsync(string directory, string fileName, string contentType);
        Task<string> GetPresignedGetUrlAsync(string filePath);
        string GetPublicUrl(string filePath);
        Task<string> GetTestPresignedUrlAsync();
        
        // Upload file content directly (string/JSON)
        Task<string> UploadFileContentAsync(string directory, string fileName, string content, string contentType);
        
        // Download file content directly
        Task<string> DownloadFileContentAsync(string filePath);
    }
}
