using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Data;
using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
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

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Collection)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(products);
        }

        // GET: /Admin/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductFormViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Collections = await _context.Collections.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Sizes = await _context.Sizes.ToListAsync()
            };

            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(viewModel);
        }

        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    ProductType = model.ProductType,
                    IsAvailable = model.IsAvailable,
                    IsNewThisWeek = model.IsNewThisWeek,
                    IsFeatured = model.IsFeatured,
                    CategoryId = model.CategoryId,
                    CollectionId = model.CollectionId,
                    CreatedAt = DateTime.Now
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Ajouter les couleurs
                foreach (var colorId in model.SelectedColorIds)
                {
                    _context.ProductColors.Add(new ProductColor
                    {
                        ProductId = product.Id,
                        ColorId = colorId
                    });
                }

                // Ajouter les tailles
                foreach (var sizeId in model.SelectedSizeIds)
                {
                    _context.ProductSizes.Add(new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = sizeId,
                        Stock = 10
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.Categories = await _context.Categories.ToListAsync();
            model.Collections = await _context.Collections.ToListAsync();
            model.Colors = await _context.Colors.ToListAsync();
            model.Sizes = await _context.Sizes.ToListAsync();
            
            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(model);
        }

        // GET: /Admin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CartCount = await GetCartItemCountAsync();
            var viewModel = new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                ProductType = product.ProductType,
                IsAvailable = product.IsAvailable,
                IsNewThisWeek = product.IsNewThisWeek,
                IsFeatured = product.IsFeatured,
                CategoryId = product.CategoryId,
                CollectionId = product.CollectionId,
                SelectedColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
                SelectedSizeIds = product.ProductSizes.Select(ps => ps.SizeId).ToList(),
                Categories = await _context.Categories.ToListAsync(),
                Collections = await _context.Collections.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Sizes = await _context.Sizes.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: /Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await _context.Products
                    .Include(p => p.ProductColors)
                    .Include(p => p.ProductSizes)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.ImageUrl = model.ImageUrl;
                product.ProductType = model.ProductType;
                product.IsAvailable = model.IsAvailable;
                product.IsNewThisWeek = model.IsNewThisWeek;
                product.IsFeatured = model.IsFeatured;
                product.CategoryId = model.CategoryId;
                product.CollectionId = model.CollectionId;

                // Supprimer les anciennes couleurs et tailles
                _context.ProductColors.RemoveRange(product.ProductColors);
                _context.ProductSizes.RemoveRange(product.ProductSizes);

                // Ajouter les nouvelles couleurs
                foreach (var colorId in model.SelectedColorIds)
                {
                    _context.ProductColors.Add(new ProductColor
                    {
                        ProductId = product.Id,
                        ColorId = colorId
                    });
                }

                // Ajouter les nouvelles tailles
                foreach (var sizeId in model.SelectedSizeIds)
                {
                    _context.ProductSizes.Add(new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = sizeId,
                        Stock = 10
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.Categories = await _context.Categories.ToListAsync();
            model.Collections = await _context.Collections.ToListAsync();
            model.Colors = await _context.Colors.ToListAsync();
            model.Sizes = await _context.Sizes.ToListAsync();
            
            ViewBag.CartCount = await GetCartItemCountAsync();
            return View(model);
        }

        // POST: /Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                _context.ProductColors.RemoveRange(product.ProductColors);
                _context.ProductSizes.RemoveRange(product.ProductSizes);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
