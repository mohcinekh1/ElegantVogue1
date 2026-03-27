using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ElegantVogue.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElegantVogue.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // context.Database.EnsureDeleted(); // Reset effectué
            context.Database.Migrate();

            // Créer le rôle Admin s'il n'existe pas
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Créer le rôle Client s'il n'existe pas
            if (!await roleManager.RoleExistsAsync("Client"))
            {
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            // Créer l'utilisateur Admin s'il n'existe pas
            if (await userManager.FindByEmailAsync("admin@elegantvogue.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@elegantvogue.com",
                    Email = "admin@elegantvogue.com",
                    FirstName = "Admin",
                    LastName = "ElegantVogue",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Si des produits existent déjà, ne pas réinitialiser
            if (context.Products.Any())
            {
                return;
            }

            // SIZES
            var sizes = new Size[]
            {
                new Size { Name = "XS", SortOrder = 1 },
                new Size { Name = "S", SortOrder = 2 },
                new Size { Name = "M", SortOrder = 3 },
                new Size { Name = "L", SortOrder = 4 },
                new Size { Name = "XL", SortOrder = 5 },
                new Size { Name = "2X", SortOrder = 6 }
            };
            context.Sizes.AddRange(sizes);
            await context.SaveChangesAsync();

            // COLORS
            var colors = new Color[]
            {
                new Color { Name = "Black", HexCode = "#000000" },
                new Color { Name = "White", HexCode = "#FFFFFF" },
                new Color { Name = "Gray", HexCode = "#808080" },
                new Color { Name = "Cream", HexCode = "#FFFDD0" },
                new Color { Name = "Navy", HexCode = "#000080" },
                new Color { Name = "Mint", HexCode = "#98FF98" },
                new Color { Name = "Lavender", HexCode = "#E6E6FA" },
                new Color { Name = "Olive", HexCode = "#808000" },
                new Color { Name = "Camo", HexCode = "#78866B" }
            };
            context.Colors.AddRange(colors);
            await context.SaveChangesAsync();

            // CATEGORIES
            var categories = new Category[]
            {
                new Category { Name = "Men", Description = "Men's clothing" },
                new Category { Name = "Women", Description = "Women's clothing" },
                new Category { Name = "Kid", Description = "Kids' clothing" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // COLLECTIONS
            var collections = new Collection[]
            {
                new Collection { Name = "Summer 2024", Description = "Summer collection", Season = "Summer", Year = 2024, IsActive = true },
                new Collection { Name = "XIV Collections 23-24", Description = "XIV collection", Season = "Winter", Year = 2024, IsActive = true }
            };
            context.Collections.AddRange(collections);
            await context.SaveChangesAsync();

            // PRODUCTS
            var products = new Product[]
            {
                new Product
                {
                    Name = "Abstract Print Shirt",
                    Description = "Relaxed-fit shirt. Camp collar and short sleeves. Button-up front.",
                    Price = 99.00m,
                    ImageUrl = "/images/products/ton-vrai-nom.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[0].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Basic Slim Fit T-Shirt",
                    Description = "Essential slim fit t-shirt in premium cotton.",
                    Price = 199.00m,
                    ImageUrl = "/images/products/tshirt-cream-basic.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[0].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Basic Heavy Weight T-Shirt",
                    Description = "Premium heavyweight cotton t-shirt with relaxed fit.",
                    Price = 199.00m,
                    ImageUrl = "/images/products/tshirt-black-heavy.jpg",
                    ProductType = "Crewneck T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[1].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = false
                },
                new Product
                {
                    Name = "Full Sleeve Zipper",
                    Description = "Stylish full sleeve shirt with zipper detail.",
                    Price = 199.00m,
                    ImageUrl = "/images/products/shirt-pattern.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[1].Id,
                    IsAvailable = true,
                    IsFeatured = false,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Embroidered Seersucker Shirt",
                    Description = "V-neck t-shirt with embroidered details.",
                    Price = 99.00m,
                    ImageUrl = "/images/products/tshirt-wave.jpg",
                    ProductType = "V Neck T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[0].Id,
                    IsAvailable = true,
                    IsFeatured = false,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Base Slim Fit T-Shirt",
                    Description = "Classic slim fit cotton t-shirt.",
                    Price = 99.00m,
                    ImageUrl = "/images/products/tshirt-ink.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[0].Id,
                    IsAvailable = true,
                    IsFeatured = false,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Blurred Print T-Shirt",
                    Description = "Henley t-shirt with unique blurred print design.",
                    Price = 99.00m,
                    ImageUrl = "/images/products/tshirt-henley.jpg",
                    ProductType = "Henley T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[0].Id,
                    IsAvailable = true,
                    IsFeatured = false,
                    IsNewThisWeek = true
                },
                new Product
                {
                    Name = "Soft Wash Straight Fit Jeans",
                    Description = "Comfortable straight fit jeans with soft wash finish.",
                    Price = 199.00m,
                    ImageUrl = "/images/products/jeans-white.jpg",
                    ProductType = "Cotton Jeans",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[1].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = false
                },
                new Product
                {
                    Name = "Basic Heavy Weight T-Shirt Camo",
                    Description = "Premium heavyweight cotton t-shirt in camo pattern.",
                    Price = 199.00m,
                    ImageUrl = "/images/products/tshirt-camo2.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[1].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = false
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // PRODUCT COLORS
            var productColors = new ProductColor[]
            {
                new ProductColor { ProductId = products[0].Id, ColorId = colors[0].Id },
                new ProductColor { ProductId = products[0].Id, ColorId = colors[1].Id },
                new ProductColor { ProductId = products[0].Id, ColorId = colors[2].Id },
                new ProductColor { ProductId = products[1].Id, ColorId = colors[3].Id },
                new ProductColor { ProductId = products[1].Id, ColorId = colors[0].Id },
                new ProductColor { ProductId = products[2].Id, ColorId = colors[0].Id },
                new ProductColor { ProductId = products[3].Id, ColorId = colors[7].Id },
                new ProductColor { ProductId = products[4].Id, ColorId = colors[1].Id },
                new ProductColor { ProductId = products[5].Id, ColorId = colors[0].Id },
                new ProductColor { ProductId = products[6].Id, ColorId = colors[2].Id },
                new ProductColor { ProductId = products[7].Id, ColorId = colors[1].Id },
                new ProductColor { ProductId = products[8].Id, ColorId = colors[8].Id }
            };
            context.ProductColors.AddRange(productColors);
            await context.SaveChangesAsync();

            // PRODUCT SIZES
            var productSizeEntries = new List<ProductSize>();
            foreach (var product in products)
            {
                foreach (var size in sizes)
                {
                    productSizeEntries.Add(new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = size.Id,
                        Stock = 10
                    });
                }
            }
            context.ProductSizes.AddRange(productSizeEntries);
            await context.SaveChangesAsync();
        }
    }
}