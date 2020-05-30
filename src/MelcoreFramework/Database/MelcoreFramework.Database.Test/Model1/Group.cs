using MelcoreFramework.Database.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model1
{
    public partial class Group : IEntity<long>
    {
        #region Public Constructors

        public Group()
        {
            Product = new HashSet<Product>();
        }

        #endregion Public Constructors



        #region Public Properties

        [Key]
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("Group")]
        public virtual ICollection<Product> Product { get; set; }

        #endregion Public Properties
    }
}