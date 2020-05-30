using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model1
{
    public partial class Product : IEntity<long>
    {
        #region Public Constructors

        public Product()
        {
            Images = new HashSet<Images>();
            Price = new HashSet<Price>();
        }

        #endregion Public Constructors



        #region Public Properties

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Product")]
        public virtual Category Category { get; set; }

        public long? CategoryId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("Product")]
        public virtual Group Group { get; set; }

        public long? GroupId { get; set; }

        [Key]
        public long Id { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Images> Images { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Price> Price { get; set; }

        #endregion Public Properties
    }
}