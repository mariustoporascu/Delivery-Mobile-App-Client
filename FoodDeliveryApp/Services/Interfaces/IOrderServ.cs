using FoodDeliveryApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IOrderServ
    {
        Task<int> CreateOrder(Order order);
        Task CreateOrderInfo(OrderInfo orderInfo);
        Task CreateProductsInOrder(List<ProductInOrder> productsInOrder);
    }
}
