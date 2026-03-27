using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
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

        public async Task<int> GetCartItemCountInternalAsync()
        {
            var cartId = GetCartId();
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<IActionResult> Index()
        {
            var cartId = GetCartId();

            var items = await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Color)
                .Include(ci => ci.Size)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            var viewModel = new CartViewModel
            {
                Items = items.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ProductType = ci.Product?.ProductType ?? "",
                    ImageUrl = ci.Product?.ImageUrl,
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    ColorName = ci.Color?.Name,
                    ColorHex = ci.Color?.HexCode,
                    SizeName = ci.Size?.Name
                }).ToList()
            };

            viewModel.Subtotal = viewModel.Items.Sum(i => i.TotalPrice);
            ViewBag.CartCount = await GetCartItemCountInternalAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1, int? colorId = null, int? sizeId = null)
        {
            var cartId = GetCartId();

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cartId &&
                    ci.ProductId == productId &&
                    ci.ColorId == colorId &&
                    ci.SizeId == sizeId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    ColorId = colorId,
                    SizeId = sizeId
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartId = GetCartId();
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CartId == cartId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    _context.CartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var cartId = GetCartId();
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CartId == cartId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cartId = GetCartId();
            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await GetCartItemCountInternalAsync();
            return Json(new { count });
        }
    }
}