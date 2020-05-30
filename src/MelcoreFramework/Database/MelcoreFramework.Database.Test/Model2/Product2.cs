using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class Product2 : IEntity<long>
    {
        #region Public Constructors

        public Product2()
        {
            Images = new HashSet<Images2>();
            Price = new HashSet<Price2>();
        }

        #endregion Public Constructors



        #region Public Properties

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Product")]
        public virtual Category2 Category { get; set; }

        public long? CategoryId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("Product")]
        public virtual Group2 Group { get; set; }

        public long? GroupId { get; set; }

        [Key]
        public long Id { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Images2> Images { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Price2> Price { get; set; }

        #endregion Public Properties
    }
}