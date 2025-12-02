using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
