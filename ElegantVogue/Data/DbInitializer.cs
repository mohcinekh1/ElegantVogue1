using ElegantVogue.Models;

namespace ElegantVogue.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

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
            context.SaveChanges();

            // COLORS
            var colors = new Color[]
            {
                new Color { Name = "Black", HexCode = "#000000" },
                new Color { Name = "White", HexCode = "#FFFFFF" },
                new Color { Name = "Gray", HexCode = "#808080" },
                new Color { Name = "Cream", HexCode = "#F5F5DC" },
                new Color { Name = "Navy", HexCode = "#000080" },
                new Color { Name = "Mint", HexCode = "#98FF98" },
                new Color { Name = "Lavender", HexCode = "#E6E6FA" },
                new Color { Name = "Olive", HexCode = "#808000" },
                new Color { Name = "Camo", HexCode = "#78866B" }
            };
            context.Colors.AddRange(colors);
            context.SaveChanges();

            // CATEGORIES
            var categories = new Category[]
            {
                new Category { Name = "Men", Description = "Men's clothing" },
                new Category { Name = "Women", Description = "Women's clothing" },
                new Category { Name = "Kid", Description = "Kids clothing" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // COLLECTIONS
            var collections = new Collection[]
            {
                new Collection
                {
                    Name = "Summer 2024",
                    Description = "New Collection Summer 2024",
                    Season = "Summer",
                    Year = "2024",
                    IsActive = true
                },
                new Collection
                {
                    Name = "XIV Collections 23-24",
                    Description = "XIV Collections 2023-2024",
                    Season = "All",
                    Year = "2023-2024",
                    IsActive = true
                }
            };
            context.Collections.AddRange(collections);
            context.SaveChanges();

            // PRODUCTS
            var products = new Product[]
            {
                new Product
                {
                    Name = "Abstract Print Shirt",
                    Description = "Relaxed-fit shirt. Camp collar and short sleeves. Button-up front.",
                    Price = 99.00m,
                    ImageUrl = "/images/products/tshirt-black-abstract.jpg",
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
                    ImageUrl = "/images/products/tshirt-camo.jpg",
                    ProductType = "Cotton T-Shirt",
                    CategoryId = categories[0].Id,
                    CollectionId = collections[1].Id,
                    IsAvailable = true,
                    IsFeatured = true,
                    IsNewThisWeek = false
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // PRODUCT COLORS
            var productColors = new List<ProductColor>();

            // Abstract Print Shirt colors
            productColors.Add(new ProductColor { ProductId = products[0].Id, ColorId = colors[0].Id });
            productColors.Add(new ProductColor { ProductId = products[0].Id, ColorId = colors[2].Id });
            productColors.Add(new ProductColor { ProductId = products[0].Id, ColorId = colors[5].Id });
            productColors.Add(new ProductColor { ProductId = products[0].Id, ColorId = colors[1].Id });
            productColors.Add(new ProductColor { ProductId = products[0].Id, ColorId = colors[6].Id });

            // Basic Slim Fit T-Shirt colors
            productColors.Add(new ProductColor { ProductId = products[1].Id, ColorId = colors[3].Id });
            productColors.Add(new ProductColor { ProductId = products[1].Id, ColorId = colors[0].Id });
            productColors.Add(new ProductColor { ProductId = products[1].Id, ColorId = colors[1].Id });

            // Other products colors
            for (int i = 2; i < products.Length; i++)
            {
                productColors.Add(new ProductColor { ProductId = products[i].Id, ColorId = colors[0].Id });
                productColors.Add(new ProductColor { ProductId = products[i].Id, ColorId = colors[1].Id });
            }

            context.ProductColors.AddRange(productColors);
            context.SaveChanges();

            // PRODUCT SIZES
            var productSizes = new List<ProductSize>();

            foreach (var product in products)
            {
                foreach (var size in sizes)
                {
                    productSizes.Add(new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = size.Id,
                        Stock = 10
                    });
                }
            }

            context.ProductSizes.AddRange(productSizes);
            context.SaveChanges();
        }
    }
}