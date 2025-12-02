using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models
{
    public class CartDetail
    {
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }


    }
}
