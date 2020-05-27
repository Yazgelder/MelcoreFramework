using MelcoreFramework.FileStored.Parameter;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MelcoreFramework.FileStored.Minio.Concrete
{
    public class MinioFileStored : IFileSystem
    {
        private readonly FileSystemParameter _fileParameter;
        private readonly MinioClient _minioClient;

        public MinioFileStored(IOptions<FileSystemParameter> fileParameter)
        {
            _fileParameter = fileParameter.Value;

            _minioClient = new MinioClient(_fileParameter.EndPoint, _fileParameter.AccessKey, _fileParameter.SecretKey);
            if (_fileParameter.IsSSL)
                _minioClient = _minioClient.WithSSL();
        }

        public Task DeleteAll()
        {
            var list = this.GetList().Result;
            list.AsParallel().ForAll((x) => { DeleteFile(x.Name).Wait(); });
            return Task.CompletedTask;
        }

        public Task<bool> DeleteFile(string name)
        {
            try
            {
                _minioClient.RemoveObjectAsync(_fileParameter.Bucket, name).Wait();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<Stream> Get(string name)
        {
            MemoryStream memoryStream = new MemoryStream();

            await _minioClient.GetObjectAsync(_fileParameter.Bucket, name, (stream) => stream.CopyTo(memoryStream));

            return memoryStream;
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
                await _minioClient.StatObjectAsync(_fileParameter.Bucket, name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task SaveFile(Stream stream, string name)
        {
            CheckAndCreadBucket(_fileParameter.Bucket);
            stream.Position = 0;
            _minioClient.PutObjectAsync(
                _fileParameter.Bucket,
                name,
                stream,
                stream.Length,
                "application/octet-stream").Wait();
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
            var fullName = $"http{(_fileParameter.IsSSL ? "s" : "")}://{_fileParameter.EndPoint}/{_fileParameter.Bucket}";
            if (string.IsNullOrEmpty(prefix))
                prefix = null;
            var lst = _minioClient.ListObjectsAsync(_fileParameter.Bucket, prefix, true);
            var t = lst.ToEnumerable().Select(item => new FileInfo() { BucketName = _fileParameter.Bucket, Name = item.Key, Size = Convert.ToInt64(item.Size), FullName = fullName + "/" + item.Key });
            list.AddRange(t.ToList());
            return list;
        }

        private void CheckAndCreadBucket(string bucket)
        {
            bool found = _minioClient.BucketExistsAsync(bucket).Result;
            if (!found)
            {
                _minioClient.MakeBucketAsync(bucket);
            }
        }
    }
}