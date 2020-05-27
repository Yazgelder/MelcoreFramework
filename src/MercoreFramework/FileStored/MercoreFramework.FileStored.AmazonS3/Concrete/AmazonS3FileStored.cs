using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MercoreFramework.FileStored.Parameter;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MercoreFramework.FileStored.AmazonS3.Concrete
{
    public class AmazonS3FileStored : IFileSystem
    {
        private readonly FileSystemParameter _fileParameter;
        private readonly AmazonS3Client _s3Client;

        private int MB
        {
            get { return Convert.ToInt32(Math.Pow(2, 20)); }
        }

        public AmazonS3FileStored(IOptions<FileSystemParameter> fileParameter)
        {
            _fileParameter = fileParameter.Value;
            var endpoint = RegionEndpoint.EnumerableAllRegions.FirstOrDefault(x => x.SystemName == _fileParameter.EndPoint);
            var q = new AmazonS3Config
            {
                RegionEndpoint = endpoint,
                Timeout = TimeSpan.FromSeconds(10),            // Default value is 100 seconds
                ReadWriteTimeout = TimeSpan.FromSeconds(10),   // Default value is 300 seconds
                MaxErrorRetry = 2                              // Default value is 4 retries
            };

            _s3Client = new AmazonS3Client(_fileParameter.AccessKey, _fileParameter.SecretKey, q);
        }

        public Task DeleteAll()
        {
            var list = this.GetList().Result;
            list.AsParallel().ForAll((x) => { DeleteFile(x.Name).Wait(); });
            return Task.CompletedTask;
        }

        public Task<bool> DeleteFile(string name)
        {
            DeleteObjectRequest delete = new DeleteObjectRequest()
            {
                BucketName = _fileParameter.Bucket,
                Key = name
            };

            var r = _s3Client.DeleteObjectAsync(delete).Result;
            if (r.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public async Task<Stream> Get(string name)
        {
            Amazon.S3.Model.GetObjectRequest getObject = new GetObjectRequest()
            {
                Key = name,
                BucketName = _fileParameter.Bucket,
            };
            var r = await _s3Client.GetObjectAsync(getObject);
            if (r.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                MemoryStream memoryStream = new MemoryStream();

                using (Stream responseStream = r.ResponseStream)
                {
                    responseStream.CopyTo(memoryStream);
                }
                return memoryStream;
            }
            return null;
        }

        public Task<List<IFileInfo>> GetList()
        {
            return Task.FromResult(GetDirectories(""));
        }

        public Task<List<IFileInfo>> GetList(string prefix)
        {
            return Task.FromResult(GetDirectories(prefix));
        }

        public async Task<bool> IsExities(string name)
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest()
                {
                    Key = name,
                    BucketName = _fileParameter.Bucket,
                });
                return true;
            }
            catch (Amazon.S3.AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                throw;
            }
        }

        public Task SaveFile(Stream stream, string name)
        {
            stream.Position = 0;
            System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken(false);
            var t = new List<PartETag>();

            decimal count = stream.Length / (5 * (decimal)MB);
            var i = (long)count;
            if (Convert.ToDecimal(i) != count)
                i++;

            InitiateMultipartUploadRequest initRequest = new InitiateMultipartUploadRequest
            {
                BucketName = _fileParameter.Bucket,
                Key = name
            };

            initRequest.CannedACL = S3CannedACL.PublicRead;
            InitiateMultipartUploadResponse initResponse = _s3Client.InitiateMultipartUploadAsync(initRequest, cancellationToken).Result;

            for (int j = 1; j <= i; j++)
            {
                UploadPartRequest uploadRequest = new UploadPartRequest
                {
                    BucketName = initRequest.BucketName,
                    Key = initRequest.Key,
                    UploadId = initResponse.UploadId,
                    PartNumber = j,
                };

                if (j != i)
                    uploadRequest.PartSize = 5 * MB;
                uploadRequest.InputStream = stream;
                UploadPartResponse up1Response = _s3Client.UploadPartAsync(uploadRequest, cancellationToken).Result;
                t.Add(new PartETag { ETag = up1Response.ETag, PartNumber = j });
            }

            CompleteMultipartUploadRequest compRequest = new CompleteMultipartUploadRequest
            {
                BucketName = initRequest.BucketName,
                Key = initRequest.Key,
                UploadId = initResponse.UploadId,
                PartETags = t
            };

            _s3Client.CompleteMultipartUploadAsync(compRequest, cancellationToken).Wait();
            return Task.CompletedTask;
        }

        public Task SaveFile(string tempName, string name)
        {
            using (var fileToUpload = new FileStream(tempName, FileMode.Open, FileAccess.Read))
            {
                SaveFile(fileToUpload, name).Wait();
            }
            return Task.CompletedTask;
        }

        private List<IFileInfo> GetDirectories(string prefix)
        {
            List<IFileInfo> list = new List<IFileInfo>();
            var fullName = $"https://{_fileParameter.Bucket}.s3.{_fileParameter.EndPoint}.amazonaws.com";

            ListObjectsV2Request r = new ListObjectsV2Request()
            {
                BucketName = _fileParameter.Bucket,
                Prefix = prefix,
            };

            ListObjectsV2Response rtn = null;
            do
            {
                if (rtn != null)
                    r.ContinuationToken = rtn.ContinuationToken;
                rtn = _s3Client.ListObjectsV2Async(r).Result;

                list.AddRange(rtn.S3Objects.Select(x => (IFileInfo)new FileInfo() { BucketName = x.BucketName, Name = x.Key, Size = x.Size, FullName = fullName + "/" + x.Key }));
            } while (rtn.IsTruncated);

            return list;
        }
    }
}