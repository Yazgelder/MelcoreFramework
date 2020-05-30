using MelcoreFramework.Database.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model2
{
    public partial class Price2 : IEntity<long>
    {
        #region Public Properties

        [Key]
        public long Id { get; set; }

        [Column("Price", TypeName = "money")]
        public decimal? Price1 { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Price")]
        public virtual Product2 Product { get; set; }

        public long? ProductId { get; set; }

        #endregion Public Properties
    }
}