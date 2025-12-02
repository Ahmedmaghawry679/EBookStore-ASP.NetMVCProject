namespace BookStoreMVCUI.Models.DTOs
{
    public class OrderDetailsModelDTO
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
