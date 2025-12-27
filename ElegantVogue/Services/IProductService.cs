using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Services
{
    public interface IProductService
    {
        Task<HomeViewModel> GetHomeViewModelAsync();
        Task<ProductListViewModel> GetProductListViewModelAsync(
            string? search,
            int? categoryId,
            int? collectionId,
            List<int>? sizeIds,
            List<int>? colorIds,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStockOnly,
            string? sortBy);
        Task<ProductDetailViewModel?> GetProductDetailViewModelAsync(int id);
        Task<Product?> GetProductByIdAsync(int id);
    }
}