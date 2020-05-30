using MelcoreFramework.Database.Abstract;
using MelcoreFramework.Database.Concrete;
using MelcoreFramework.Database.Test.Model1;

namespace MelcoreFramework.Database.Test.DataLayer
{
    public interface IProductDal : IEntityRepository<Product, long>
    {
    }

    public class ProductDal : EntityRepository<TestContext, Product, long>, IProductDal
    {
        #region Public Constructors

        public ProductDal(TestContext testContext)
            : base(testContext)
        {
        }

        #endregion Public Constructors
    }
}