using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
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

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Collection)
                    .Where(p => p.IsFeatured && p.IsAvailable)
                    .Take(4)
                    .ToListAsync(),

                NewThisWeek = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.Color)
                    .Where(p => p.IsNewThisWeek && p.IsAvailable)
                    .Take(4)
                    .ToListAsync(),

                CollectionProducts = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Collection)
                    .Where(p => p.CollectionId != null && p.IsAvailable)
                    .Take(6)
                    .ToListAsync(),

                CurrentCollection = await _context.Collections
                    .FirstOrDefaultAsync(c => c.IsActive)
            };

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(viewModel);
        }
    }
}