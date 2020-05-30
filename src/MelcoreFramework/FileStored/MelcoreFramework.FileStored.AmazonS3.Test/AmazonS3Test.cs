using MelcoreFramework.FileStored.AmazonS3.Concrete;
using MelcoreFramework.FileStored.Parameter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MelcoreFramework.FileStored.AmazonS3.Test
{
    public class AmazonS3Test
    {
        #region Private Fields

        private const string dllName = "MelcoreFramework.dll";
        private readonly string directory;
        private readonly List<string> fileName = null;
        private readonly ServiceProvider serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public AmazonS3Test()
        {
            var services = new ServiceCollection();
            services.AddScoped<IFileSystem, AmazonS3FileStored>();
            var opt = Options.Create(new FileSystemParameter()
            {
                AccessKey = "-----------------------------",
                EndPoint = "eu-central-1",
                SecretKey = "-------------------------------------------------------",
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

        #endregion Public Constructors



        #region Public Methods

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

        [Test, Order(4)]
        public void GetList()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            var lst = service.GetList().Result;
            Assert.AreEqual(fileName.Count + 1, lst.Count);
            lst = service.GetList(directory + @"/MelcoreFramework").Result;
            Assert.AreEqual(1, lst.Count);
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

        [Test, Order(2)]
        public void SavePhysicalFile()
        {
            var service = serviceProvider.GetService<IFileSystem>();
            service.SaveFile(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, dllName), directory + @"/" + dllName).Wait();

            Assert.Pass();
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

        #endregion Public Methods
    }
}