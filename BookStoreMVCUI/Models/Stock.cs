namespace BookStoreMVCUI.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int BookId {  get; set; }
        public virtual Book? Book { get; set; }
        public int Quantity { get; set; }

    }
}
