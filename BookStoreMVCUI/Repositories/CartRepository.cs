
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;

namespace BookStoreMVCUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public CartRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _dbContext.Database.BeginTransaction(); // Make a Transaction Because We Editing in Multiple Tables If Fail in Any Thing Roll Back all and This For Atomicity Principle in ACID
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User Is Not Looged-In");
                else
                {
                    var cart = await GetCart(userId);
                    if(cart is null)
                    {
                        cart = new ShoppingCart();
                        cart.UserId = userId;
                        _dbContext.ShoppingCarts.Add(cart); // First Table
                    }
                    _dbContext.SaveChanges();

                     var cartItem = _dbContext.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.Id && x.BookId == bookId);
                    if (cartItem != null)
                        cartItem.Quantity += qty;
                    else
                    {
                        var book = _dbContext.Books.Find(bookId); // Second Table
                        cartItem = new CartDetail();
                        cartItem.BookId = bookId;
                        cartItem.ShoppingCartId = cart.Id;
                        cartItem.Quantity = qty;
                        cartItem.UnitPrice = book.Price;

                        _dbContext.CartDetails.Add(cartItem); // Third Table 
                    }
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    
                }
            }
            catch (Exception ex)
            {

            }

            var cartItemCount = await GetCartItemsCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                if(string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User Is Not Looged-In");

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");

                var cartItem = _dbContext.CartDetails.FirstOrDefault(cd => cd.ShoppingCartId == cart.Id && cd.BookId == bookId);
                if (cartItem is null)
                    throw new InvalidOperationException("Invalid cart");
                else if (cartItem.Quantity == 1)
                    _dbContext.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity -= 1;

                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemsCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("Invalid User");

            var cart = await _dbContext.ShoppingCarts
                            .Include(s => s.CartDetails)
                            .ThenInclude(cd => cd.Book)
                            .ThenInclude(b => b.genre)
                            .Where(s => s.UserId == userId).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<int> GetCartItemsCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
                userId = GetUserId();

            var data = await _dbContext.CartDetails
                .Where(cd => cd.ShoppingCart.UserId == userId)
                .Select(cd=>cd.Id).ToListAsync();
            return data.Count;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }


        public async Task<bool> CheckOut(CheckoutModel model)
        {
            /// Logic
            /// Move Data From CartDetails to Order and OrderDetails Then Remove CartDetails 
            
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                string userId = GetUserId();
                if(string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User Is Not Looged-In");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid Cart");
                var cartItems = _dbContext.CartDetails.Where(cd => cd.ShoppingCartId == cart.Id).ToList();
                if (cartItems.Count() == 0)
                    throw new InvalidOperationException("Cart is Empty");

                var pendingRecord = _dbContext.OrderStatuses.FirstOrDefault(os => os.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new Exception("Order Status Does Not Have Pending Status");

                var order = new Order();
                order.UserId = userId;
                order.CreatedDate = DateTime.UtcNow;
                order.OrderStatusId = pendingRecord.Id; // Pending
                // ------------------------------
                order.Name = model.Name;
                order.Address = model.Address;
                order.PaymentMethod = model.PaymentMethod;
                order.Email = model.Email;
                order.MobileNumber = model.MobileNumber;
                order.IsPaid = false;
                // ------------------------------
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                foreach (var item in cartItems)
                {
                    var orderItem = new OrderDetail();
                    orderItem.OrderId = order.Id;
                    orderItem.BookId = item.BookId;
                    orderItem.Quantity = item.Quantity;
                    orderItem.UnitPrice = item.UnitPrice;   

                    _dbContext.OrderDetails.Add(orderItem);
                }
                _dbContext.SaveChanges();

                _dbContext.CartDetails.RemoveRange(cartItems); 
                _dbContext.SaveChanges();



                transaction.Commit();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }



        private string GetUserId()
        {
            var principle = _httpContextAccessor.HttpContext.User; // Claims Principle
            string userId = _userManager.GetUserId(principle);
            return userId;
        }

    }
}
