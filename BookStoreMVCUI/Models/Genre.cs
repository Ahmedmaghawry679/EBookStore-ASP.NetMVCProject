using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVCUI.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string GenreName {  get; set; }

        public virtual List<Book> Books { get; set; }
    }
}
