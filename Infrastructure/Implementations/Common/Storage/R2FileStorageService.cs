using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.Common.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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


        public Task<string> GetPresignedPutUrlAsync(string filePath, string contentType)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filePath,
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddMinutes(5), // The URL is valid for 5 minutes
                ContentType = contentType
            };

            string presignedUrl = _s3Client.GetPreSignedURL(request);

            return Task.FromResult(presignedUrl);
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




        [Obsolete("Use PresignedPutUrl instead.", true)]
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

            // You'll need to set up a public domain for your bucket in the Cloudflare dashboard for this to work.
            // For example, if you set up "media.yourdomain.com" to point to your bucket.
            // For now, let's return a path that assumes a public domain is configured.
            return $"https://pub-59cfd11e5f0d4b00af54839edc83842d.r2.dev/{filePath}";
            // Or just return the filePath and construct the full URL in the client/service.
            //return filePath;
        }


        public async Task<string> GetTestPresignedUrlAsync()
        {
            var testPath = $"test/connection-test-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            return await GetPresignedPutUrlAsync(testPath, "text/plain");
        }
    }
}