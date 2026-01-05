using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
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

        private async Task<CartViewModel> GetCartInternalAsync()
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

        public async Task<IActionResult> Index()
        {
            var cart = await GetCartInternalAsync();

            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new CheckoutViewModel
            {
                Cart = cart,
                CurrentStep = "information"
            };

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = await GetCartInternalAsync();
            model.Cart = cart;

            if (!ModelState.IsValid)
            {
                ViewBag.CartCount = await GetCartItemCountAsync();
                return View("Index", model);
            }

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.Now,
                Subtotal = cart.Subtotal,
                ShippingCost = cart.Shipping,
                Total = cart.Total,
                Status = "Pending",
                Email = model.Email,
                Phone = model.Phone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                StateRegion = model.StateRegion,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    ColorName = item.ColorName,
                    SizeName = item.SizeName
                };
                _context.OrderItems.Add(orderItem);
            }
            await _context.SaveChangesAsync();

            // Clear Cart
            var cartId = GetCartId();
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { orderId = order.Id });
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(order);
        }

        private string GenerateOrderNumber()
        {
            return $"EV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
