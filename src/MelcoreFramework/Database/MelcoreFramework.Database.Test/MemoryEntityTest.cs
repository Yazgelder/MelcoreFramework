using MelcoreFramework.Database.Abstract;
using MelcoreFramework.Database.Concrete;
using MelcoreFramework.Database.Test.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;

namespace MelcoreFramework.Database.Test
{
    public class MemoryEntityTest
    {
        #region Private Fields

        private readonly ServiceProvider serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public MemoryEntityTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<Model1.TestContext>(options => options.UseInMemoryDatabase(databaseName: "TestContext"));
            services.AddDbContext<Model2.TestContext2>(options => options.UseInMemoryDatabase(databaseName: "TestContext2"));

            services.AddScoped<IProductDal, ProductDal>();
            services.AddScoped<IProductDal2, ProductDal2>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddSingleton<IEntityContainer, EntityContainer>();

            serviceProvider = services.BuildServiceProvider();

            var container = serviceProvider.GetService<IEntityContainer>();
            container.AddNamespaceEntity<Model1.TestContext>(typeof(Model1.Product));
            container.AddNamespaceEntity<Model2.TestContext2>(typeof(Model2.Product2));
        }

        #endregion Public Constructors



        #region Public Methods

        [Test, Order(1)]
        public void CheckInsertTwoContext()
        {

            var productDal = serviceProvider.GetService<IProductDal>();
            using (var unitOfWork = serviceProvider.GetService<IUnitOfWork<Model1.TestContext>>())
            {
                unitOfWork.BeginTransaction();
                productDal.Add(new Model1.Product());
                unitOfWork.Rollback();
            }
            var t = productDal.Table.Count();
            Assert.AreEqual(0, t);

            using (var unitOfWork = serviceProvider.GetService<IUnitOfWork<Model1.TestContext>>())
            {
                unitOfWork.BeginTransaction();
                productDal.Add(new Model1.Product());
                unitOfWork.Commit();
            }
            t = productDal.Table.Count();
            Assert.AreEqual(1, t);

            //var productDal2 = serviceProvider.GetService<IProductDal2>();
            //productDal2.Add(new Model2.Product2());
            //var t2 = productDal2.Table.Count();
            //Assert.AreEqual(1, t2);
        }

        [Test, Order(2)]
        public void CheckUnitOfWork()
        {
            var productDal = serviceProvider.GetService<IProductDal>();
            productDal.Add(new Model1.Product());
            var t = productDal.Table.Count();
            Assert.AreEqual(1, t);

            var productDal2 = serviceProvider.GetService<IProductDal2>();
            productDal2.Add(new Model2.Product2());
            var t2 = productDal2.Table.Count();
            Assert.AreEqual(1, t2);
        }

        #endregion Public Methods
    }
}