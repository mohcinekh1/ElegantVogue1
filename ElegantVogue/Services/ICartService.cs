using ElegantVogue.Models;
using ElegantVogue.Models.ViewModels;

namespace ElegantVogue.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartAsync();
        Task AddToCartAsync(int productId, int quantity, int? colorId, int? sizeId);
        Task UpdateQuantityAsync(int cartItemId, int quantity);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync();
        Task<int> GetCartItemCountAsync();
    }
}