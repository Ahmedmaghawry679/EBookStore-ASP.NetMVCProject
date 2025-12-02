using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual List<CartDetail> CartDetails { get; set; }
    }
}
