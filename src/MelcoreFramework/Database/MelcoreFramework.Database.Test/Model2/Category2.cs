using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class Category2 : IEntity<long>
    {
        #region Public Constructors

        public Category2()
        {
            InverseParent = new HashSet<Category2>();
            Product = new HashSet<Product2>();
        }

        #endregion Public Constructors



        #region Public Properties

        [Key]
        public long Id { get; set; }

        [InverseProperty(nameof(Category2.Parent))]
        public virtual ICollection<Category2> InverseParent { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Category2.InverseParent))]
        public virtual Category2 Parent { get; set; }

        public long? ParentId { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Product2> Product { get; set; }

        #endregion Public Properties
    }
}