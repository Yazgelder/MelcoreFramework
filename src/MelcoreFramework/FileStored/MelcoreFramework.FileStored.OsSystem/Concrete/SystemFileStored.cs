using MelcoreFramework.FileStored.Parameter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MelcoreFramework.FileStored.OsSystem.Concrete
{
    public class SystemFileStored : IFileSystem
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly FileSystemParameter _fileParameter;

        public SystemFileStored(IOptions<FileSystemParameter> fileParameter, IHostEnvironment hostEnvironment)
        {
            _fileParameter = fileParameter.Value;
            _hostEnvironment = hostEnvironment;
        }

        public Task DeleteAll()
        {
            var d = new DirectoryInfo(this.GetLocation());
            d.GetDirectories().AsParallel().ForAll((x) => { x.Delete(true); });
            d.GetFiles().AsParallel().ForAll((x) => { x.Delete(); });
            return Task.CompletedTask;
        }

        public Task<bool> DeleteFile(string name)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(GetLocation(), name));
            if (f.Exists)
            {
                f.Delete();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public async Task<Stream> Get(string name)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(GetLocation(), name));
            if (f.Exists)
            {
                MemoryStream mr = new MemoryStream();
                using (var s = f.OpenRead())
                {
                    await s.CopyToAsync(mr);
                }
                if (mr.Position > 0)
                    mr.Position = 0;
                return mr;
            }
            return null;
        }

        public Task<List<IFileInfo>> GetList()
        {
            return Task.FromResult(GetDirectories(this.GetLocation()));
        }

        public Task<List<IFileInfo>> GetList(string prefix)
        {
            return Task.FromResult(GetDirectories(this.GetLocation(), prefix));
        }

        public Task<bool> IsExities(string name)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(GetLocation(), name));
            return Task.FromResult(f.Exists);
        }

        public Task SaveFile(Stream stream, string name)
        {
            name = CheckLocation(name);

            System.IO.FileInfo d = new System.IO.FileInfo(name);
            if (d.Exists)
                throw new EndOfStreamException(name);

            using (var f = d.OpenWrite())
            {
                stream.CopyTo(f);
                f.Close();
            }
            return Task.CompletedTask;
        }

        public Task SaveFile(string tempName, string name)
        {
            if (!tempName.Contains(":"))
            {
                tempName = Path.Combine(GetLocation(), tempName);
            }
            name = CheckLocation(name);
            System.IO.FileInfo s = new System.IO.FileInfo(tempName);
            if (!s.Exists)
                throw new FileNotFoundException(tempName);
            System.IO.FileInfo d = new System.IO.FileInfo(name);
            if (d.Exists)
                throw new EndOfStreamException(name);

            s.CopyTo(name);
            return Task.CompletedTask;
        }

        private List<IFileInfo> GetDirectories(string directory)
        {
            List<IFileInfo> list = new List<IFileInfo>();
            var d = new DirectoryInfo(directory);
            if (!d.Exists)
            {
                return list;
            }
            d.GetDirectories().AsParallel().ForAll((x) => { list.AddRange(GetDirectories(x.FullName)); });
            d.GetFiles().AsParallel().ForAll((x) =>
            {
                list.Add(
                    new FileInfo()
                    { FullName = x.FullName, BucketName = _fileParameter.Bucket, Name = x.Name, Size = x.Length }
                    );
            }
            );
            return list;
        }

        private List<IFileInfo> GetDirectories(string directory, string prefix)
        {
            List<IFileInfo> list = new List<IFileInfo>();
            list.AddRange(GetDirectories(System.IO.Path.Combine(directory, prefix)));

            var lst = prefix.Split("/\\".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lst.Length == 1)
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                if (di.Exists)
                {
                    di.GetFiles(lst[0] + "*").AsParallel().ForAll((x) =>
                    {
                        list.Add(new FileInfo() { FullName = x.FullName, BucketName = _fileParameter.Bucket, Name = x.Name, Size = x.Length });
                    });
                }
            }
            else
            {
                var array = lst.ToList();
                array.RemoveAt(array.Count - 1);
                var dic = string.Join(@"\", array.ToArray());
                //Son kismı dosya başkangıcı ola
                DirectoryInfo di = new DirectoryInfo(Path.Combine(directory, dic));
                if (di.Exists)
                {
                    di.GetFiles(lst.Last() + "*").AsParallel().ForAll((x) =>
                    {
                        list.Add(new FileInfo() { FullName = x.FullName, BucketName = _fileParameter.Bucket, Name = x.Name, Size = x.Length });
                    });
                }
            }

            return list;
        }

        private string GetLocation()
        {
            string start;
            if (_fileParameter.Address.Contains(":"))
            {
                start = _fileParameter.Address;
            }
            else
            {
                start = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");
            }

            if (!string.IsNullOrEmpty(_fileParameter.Bucket))
            {
                return Path.Combine(start, _fileParameter.Bucket);
            }
            else
            {
                return start;
            }
        }

        private string CheckLocation(string name)
        {
            name = name.Replace("/", @"\");
            if (!name.Contains(":"))
            {
                name = Path.Combine(GetLocation(), name);
            }
            var last = name.LastIndexOf(@"\");
            if (last == -1)
            {
                throw new DirectoryNotFoundException(name);
            }
            var directory = name.Substring(0, last);
            DirectoryInfo di = new DirectoryInfo(directory);
            if (!di.Exists)
            {
                di.Create();
            }
            Debug.Write(name);
            return name;
        }
    }
}