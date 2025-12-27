using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCartId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return Guid.NewGuid().ToString();

            var cartId = session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString("CartId", cartId);
            }
            return cartId;
        }

        public async Task<CartViewModel> GetCartAsync()
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

            return viewModel;
        }

        public async Task AddToCartAsync(int productId, int quantity, int? colorId, int? sizeId)
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
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
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
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartId = GetCartId();
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CartId == cartId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            var cartId = GetCartId();
            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync()
        {
            var cartId = GetCartId();
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity);
        }
    }
}