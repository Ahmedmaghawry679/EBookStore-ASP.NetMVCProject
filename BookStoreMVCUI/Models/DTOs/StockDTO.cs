using System.ComponentModel.DataAnnotations;

namespace BookStoreMVCUI.Models.DTOs
{
    public class StockDTO
    {
        public int BookId { get; set; }
        [Range(0,int.MaxValue, ErrorMessage ="Quantity Must Be a Non Negative Value")]
        public int Quantity { get; set; }
    }
}
