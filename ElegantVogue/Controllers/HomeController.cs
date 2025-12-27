using Microsoft.AspNetCore.Mvc;
using ElegantVogue.Services;

namespace ElegantVogue.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _productService.GetHomeViewModelAsync();
            ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }
    }
}