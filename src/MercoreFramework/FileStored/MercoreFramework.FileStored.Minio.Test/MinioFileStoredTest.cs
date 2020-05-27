using MercoreFramework.FileStored.Minio.Concrete;
using MercoreFramework.FileStored.Parameter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MercoreFramework.FileStored.Minio.Test
{
    public class MinioFileStoredTest
    {
        private const string dllName = "MercoreFramework.dll";
        private readonly ServiceProvider serviceProvider;
        private readonly List<string> fileName = null;
        private readonly string directory;

        public MinioFileStoredTest()
        {
            var services = new ServiceCollection();
            services.AddScoped<IFileSystem, MinioFileStored>();
            var opt = Options.Create(new FileSystemParameter()
            {
                AccessKey = "software.moda",
                EndPoint = "cdn.software.moda",
                SecretKey = "---------------------------------------------",
                Address = "",
                Bucket = "notifysofttest",
            });
            services.AddSingleton(opt);
            Mock<IHostEnvironment> hostEnvironment = new Mock<IHostEnvironment>();
            hostEnvironment.Setup(x => x.ContentRootPath).Returns(TestContext.CurrentContext.TestDirectory);
            services.AddSingleton(hostEnvironment.Object);
            directory = DateTime.Now.Ticks.ToString();
            serviceProvider = services.BuildServiceProvider();
            fileName = new List<string>() {
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N"),
                        Guid.NewGuid().ToString("N")};
        }

        [Test, Order(1)]
        public void SaveStreamFile()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            foreach (var item in fileName)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N"));
                using MemoryStream stream = new MemoryStream(byteArray);
                service.SaveFile(stream, directory + @"/" + item + ".txt").Wait();
            }

            Assert.Pass();
        }

        [Test, Order(2)]
        public void SavePhysicalFile()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            service.SaveFile(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, dllName), directory + @"/" + dllName).Wait();

            Assert.Pass();
        }

        [Test, Order(3)]
        public void IsExities()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            var r = service.IsExities(directory + @"/" + dllName).Result;
            if (r)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test, Order(4)]
        public void GetList()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            var lst = service.GetList().Result;
            Assert.AreEqual(fileName.Count + 1, lst.Count);
            lst = service.GetList(directory + @"/MercoreFramework").Result;
            Assert.AreEqual(1, lst.Count);
        }

        [Test, Order(5)]
        public void Get()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            var s = service.Get(directory + @"/" + dllName).Result;
            if (s == null || s.Length <= 100)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test, Order(6)]
        public void DeleteFile()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            service.DeleteFile(directory + @"/" + dllName).Wait();
            var r = service.IsExities(directory + @"/" + dllName).Result;
            if (!r)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test, Order(7)]
        public void DeleteAll()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            service.DeleteAll().Wait();
            var q = service.GetList().Result;
            if (q.Count == 0)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}