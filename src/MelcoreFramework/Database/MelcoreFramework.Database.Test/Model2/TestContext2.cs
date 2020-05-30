using MelcoreFramework.Database.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class TestContext2 : DbContext
    {
        #region Private Fields

        private readonly IEntityContainer _entityContainer;

        #endregion Private Fields

        #region Public Constructors

        public TestContext2(IEntityContainer entityContainer)
        {
            _entityContainer = entityContainer;
        }

        public TestContext2(DbContextOptions<TestContext2> options, IEntityContainer entityContainer)
            : base(options)
        {
            _entityContainer = entityContainer;
        }

        #endregion Public Constructors



        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _entityContainer.SetEntity<TestContext2>(modelBuilder);
        }

        #endregion Protected Methods
    }
}