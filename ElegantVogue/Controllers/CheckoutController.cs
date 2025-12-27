using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;
using ElegantVogue.Services;

namespace ElegantVogue.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CheckoutController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();

            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new CheckoutViewModel
            {
                Cart = cart,
                CurrentStep = "information"
            };

            ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = await _cartService.GetCartAsync();
            model.Cart = cart;

            if (!ModelState.IsValid)
            {
                ViewBag.CartCount = await _cartService.GetCartItemCountAsync();
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

            await _cartService.ClearCartAsync();

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

            return View(order);
        }

        private string GenerateOrderNumber()
        {
            return $"EV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}