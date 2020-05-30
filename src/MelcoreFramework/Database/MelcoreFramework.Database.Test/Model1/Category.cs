using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model1
{
    public partial class Category : IEntity<long>
    {
        #region Public Constructors

        public Category()
        {
            InverseParent = new HashSet<Category>();
            Product = new HashSet<Product>();
        }

        #endregion Public Constructors



        #region Public Properties

        [Key]
        public long Id { get; set; }

        [InverseProperty(nameof(Category.Parent))]
        public virtual ICollection<Category> InverseParent { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Category.InverseParent))]
        public virtual Category Parent { get; set; }

        public long? ParentId { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Product> Product { get; set; }

        #endregion Public Properties
    }
}