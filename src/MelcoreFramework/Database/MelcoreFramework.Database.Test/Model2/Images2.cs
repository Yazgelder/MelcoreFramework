using MelcoreFramework.Database.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class Images2 : IEntity<long>
    {
        #region Public Properties

        [Key]
        public long Id { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Images")]
        public virtual Product2 Product { get; set; }

        public long? ProductId { get; set; }

        #endregion Public Properties
    }
}