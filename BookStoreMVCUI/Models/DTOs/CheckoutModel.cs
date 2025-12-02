using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models.DTOs
{
    public class CheckoutModel
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(40)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
    }
}
