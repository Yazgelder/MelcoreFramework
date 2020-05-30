using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class Group2 : IEntity<long>
    {
        #region Public Constructors

        public Group2()
        {
            Product = new HashSet<Product2>();
        }

        #endregion Public Constructors



        #region Public Properties

        [Key]
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("Group")]
        public virtual ICollection<Product2> Product { get; set; }

        #endregion Public Properties
    }
}