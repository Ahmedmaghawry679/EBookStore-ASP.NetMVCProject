namespace BookStoreMVCUI.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<int> GetCartItemsCount(string userId="");
        Task<ShoppingCart> GetUserCart();
        Task<ShoppingCart> GetCart(string userId = "");
        Task<bool> CheckOut(CheckoutModel model);

    }
}
