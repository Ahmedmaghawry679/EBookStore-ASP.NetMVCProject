using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVCUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            /// We're Going To Hit This Method From 2 Loacations 
            /// First is a Java Script Methed That Will Just Return a Count Of Cart 
            /// Second If We Don't Hit With the Java Script Method It Will Simply Redirect to The GetUserCart() Method

            var cartCount = await _cartRepository.AddItem(bookId, qty);
            if(redirect == 0)
                return Ok(cartCount);

            return RedirectToAction("GetUserCart");
        }
        
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepository.RemoveItem(bookId);

            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemsInCart()
        {
            int totalItemsInCart = await _cartRepository.GetCartItemsCount();
            return Ok(totalItemsInCart);
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model); 
            bool isCheckedOut = await _cartRepository.CheckOut(model);
            if (!isCheckedOut) 
                return RedirectToAction("OrderFailure");

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
