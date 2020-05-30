using MelcoreFramework.Database.Abstract;
using MelcoreFramework.Database.Concrete;
using MelcoreFramework.Database.Test.Model2;

namespace MelcoreFramework.Database.Test.DataLayer
{
    public interface IProductDal2 : IEntityRepository<Product2, long>
    {
    }

    public class ProductDal2 : EntityRepository<TestContext2, Product2, long>, IProductDal2
    {
        #region Public Constructors

        public ProductDal2(TestContext2 testContext)
            : base(testContext)
        {
        }

        #endregion Public Constructors
    }
}