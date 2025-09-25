using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Common.Storage
{
    public interface IFileStorageService
    {
        //Task<string> UploadFileAsync(string filePath, Stream fileStream, string contentType);
        Task DeleteFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);

        Task<string> GetPresignedPutUrlAsync(string filePath, string contentType);
        Task<string> GetPresignedGetUrlAsync(string filePath);
        string GetPublicUrl(string filePath);
    }
}
