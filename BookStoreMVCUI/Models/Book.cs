using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVCUI.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string BookName { get; set; }
        [Required]
        [MaxLength(40)]
        public string AuthorName { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image {  get; set; }

        public int GenreId { get; set; }
        public virtual Genre genre { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

        public virtual List<CartDetail> CartDetails { get; set; }

        [NotMapped]
        public string GenreName { get; set; }

        public Stock? Stock { get; set; }

        [NotMapped]
        public int Quantity { get; set; }

    }
}
