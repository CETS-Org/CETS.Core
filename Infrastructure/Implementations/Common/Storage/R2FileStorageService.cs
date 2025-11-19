using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.Common.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.Storage
{
    public class R2FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly CloudflareR2Settings _settings;

        public R2FileStorageService(IOptions<CloudflareR2Settings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            var config = new AmazonS3Config
            {
                ServiceURL = _settings.Endpoint,
                ForcePathStyle = true,
                UseHttp = false
            };

            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
        }


        public Task<(string PresignedUrl, string FilePath)> GetPresignedPutUrlAsync(string directory, string fileName, string contentType)
        {
            // Generate unique file path
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = $"{directory.TrimEnd('/')}/{DateTime.Now:yyyy/MM/dd}/{uniqueFileName}";

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddMinutes(5), // The URL is valid for 5 minutes
                ContentType = contentType
            };

            string presignedUrl = _s3Client.GetPreSignedURL(request);

            return Task.FromResult((presignedUrl, filePath));
        }

        public Task<string> GetPresignedGetUrlAsync(string filePath)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                Verb = HttpVerb.GET,
                Expires = DateTime.Now.AddMinutes(30), // downloads are valid for 30 minutes
                ResponseHeaderOverrides = new ResponseHeaderOverrides
                {
                    ContentDisposition = "attachment" // Force download instead of inline display
                }
            };

            string presignedUrl = _s3Client.GetPreSignedURL(request);

            return Task.FromResult(presignedUrl);
        }


        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _settings.BucketName,
                    Key = filePath
                };

                await _s3Client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public string GetPublicUrl(string filePath)
        {
            return $"{_settings.PublicUrl.TrimEnd('/')}/{filePath}";
        }

        public async Task DeleteFileAsync(string filePath)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath
            };
            await _s3Client.DeleteObjectAsync(deleteRequest);
        }




        public async Task<string> UploadFileAsync(string filePath, Stream fileStream, string contentType)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                InputStream = fileStream,
                ContentType = contentType,
                // R2 recommends setting ACL to private, as access is controlled by bucket policies
                CannedACL = S3CannedACL.Private
            };

            await _s3Client.PutObjectAsync(putRequest);

            // Return filePath (not full URL) so it can be stored in DB
            return filePath;
        }


        public async Task<string> GetTestPresignedUrlAsync()
        {
            var testFileName = $"connection-test-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            var (presignedUrl, _) = await GetPresignedPutUrlAsync("test", testFileName, "text/plain");
            return presignedUrl;
        }

        public async Task<string> UploadFileContentAsync(string directory, string fileName, string content, string contentType)
        {
            // Generate unique file path
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = $"{directory.TrimEnd('/')}/{DateTime.Now:yyyy/MM/dd}/{uniqueFileName}";

            var contentBytes = Encoding.UTF8.GetBytes(content);
            using var stream = new MemoryStream(contentBytes);

            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                InputStream = stream,
                ContentType = contentType,
                CannedACL = S3CannedACL.Private
            };

            await _s3Client.PutObjectAsync(putRequest);

            return filePath;
        }

        public async Task<string> DownloadFileContentAsync(string filePath)
        {
            var request = new GetObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath
            };

            using var response = await _s3Client.GetObjectAsync(request);
            using var reader = new StreamReader(response.ResponseStream);
            return await reader.ReadToEndAsync();
        }
    }
}