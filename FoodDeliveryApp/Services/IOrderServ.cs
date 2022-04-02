using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Text;
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
