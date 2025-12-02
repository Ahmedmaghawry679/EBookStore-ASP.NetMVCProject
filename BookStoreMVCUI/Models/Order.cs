using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        [MaxLength(40)]
        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        [Required]
        [MaxLength(30)]
        public bool IsPaid { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }


    }
}
