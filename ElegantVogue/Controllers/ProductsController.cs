using Microsoft.AspNetCore.Mvc;
using ElegantVogue.Services;

namespace ElegantVogue.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductsController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            int? collectionId,
            [FromQuery] List<int>? sizeIds,
            [FromQuery] List<int>? colorIds,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStockOnly,
            string? sortBy)
        {
            var viewModel = await _productService.GetProductListViewModelAsync(
                search, categoryId, collectionId, sizeIds, colorIds,
                minPrice, maxPrice, inStockOnly, sortBy);

            ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _productService.GetProductDetailViewModelAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }
    }
}