using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetCartId()
        {
            var cartId = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartId", cartId);
            }
            return cartId;
        }

        private async Task<int> GetCartItemCountAsync()
        {
            var cartId = GetCartId();
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity);
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
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Collection)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) ||
                                        (p.Description != null && p.Description.Contains(search)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (collectionId.HasValue)
            {
                query = query.Where(p => p.CollectionId == collectionId.Value);
            }

            if (sizeIds != null && sizeIds.Any())
            {
                query = query.Where(p => p.ProductSizes.Any(ps => sizeIds.Contains(ps.SizeId)));
            }

            if (colorIds != null && colorIds.Any())
            {
                query = query.Where(p => p.ProductColors.Any(pc => colorIds.Contains(pc.ColorId)));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (inStockOnly == true)
            {
                query = query.Where(p => p.IsAvailable);
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var products = await query.ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                Categories = await _context.Categories.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Sizes = await _context.Sizes.OrderBy(s => s.SortOrder).ToListAsync(),
                Collections = await _context.Collections.Where(c => c.IsActive).ToListAsync(),
                SearchQuery = search,
                SelectedCategoryId = categoryId,
                SelectedCollectionId = collectionId,
                SelectedSizeIds = sizeIds ?? new List<int>(),
                SelectedColorIds = colorIds ?? new List<int>(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                InStockOnly = inStockOnly,
                SortBy = sortBy,
                TotalProducts = products.Count,
                AvailableCount = products.Count(p => p.IsAvailable),
                OutOfStockCount = products.Count(p => !p.IsAvailable)
            };

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Collection)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                AvailableColors = product.ProductColors
                    .Select(pc => pc.Color!)
                    .ToList(),
                AvailableSizes = product.ProductSizes
                    .Select(ps => ps.Size!)
                    .OrderBy(s => s.SortOrder)
                    .ToList()
            };

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(viewModel);
        }
    }
}