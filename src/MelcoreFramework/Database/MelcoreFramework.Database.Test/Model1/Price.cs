using MelcoreFramework.Database.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MelcoreFramework.Database.Test.Model1
{
    public partial class Price : IEntity<long>
    {
        #region Public Properties

        [Key]
        public long Id { get; set; }

        [Column("Price", TypeName = "money")]
        public decimal? Price1 { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Price")]
        public virtual Product Product { get; set; }

        public long? ProductId { get; set; }

        #endregion Public Properties
    }
}