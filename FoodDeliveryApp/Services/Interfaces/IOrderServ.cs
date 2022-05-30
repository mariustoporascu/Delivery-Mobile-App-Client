using FoodDeliveryApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IOrderServ
    {
        Task<string> CreateOrder(ServerOrder order);
        Task CreateOrderInfo(OrderInfo orderInfo);
        Task CreateProductsInOrder(List<ProductInOrder> productsInOrder);
        Task<DriverLocation> LoadDrivers(string driverId, int orderId);
        Task<bool> AgreeEstTime(int orderId, bool accept);
        Task<bool> GiveRatingDriver(string email, int orderId, int rating);
        Task<bool> GiveRatingRestaurant(string email, int orderId, int rating);
    }
}
