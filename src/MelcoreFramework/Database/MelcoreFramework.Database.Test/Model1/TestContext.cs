using MelcoreFramework.Database.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MelcoreFramework.Database.Test.Model1
{
    public partial class TestContext : DbContext
    {
        #region Private Fields

        private readonly IEntityContainer _entityContainer;

        #endregion Private Fields

        #region Public Constructors

        public TestContext(IEntityContainer entityContainer)
        {
            _entityContainer = entityContainer;
        }

        public TestContext(DbContextOptions<TestContext> options, IEntityContainer entityContainer)
            : base(options)
        {
            _entityContainer = entityContainer;
        }

        #endregion Public Constructors



        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _entityContainer.SetEntity<TestContext>(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #endregion Protected Methods
    }
}