using Microsoft.AspNetCore.Mvc;
using ElegantVogue.Services;

namespace ElegantVogue.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _cartService.GetCartAsync();
            ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1, int? colorId = null, int? sizeId = null)
        {
            await _cartService.AddToCartAsync(productId, quantity, colorId, sizeId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await _cartService.GetCartItemCountAsync();
            return Json(new { count });
        }
    }
}