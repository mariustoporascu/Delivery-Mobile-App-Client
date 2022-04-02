using System;
using System.Collections.Generic;
using System.Text;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {
        public Order Order { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrder { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };
        public PlaceOrderViewModel()
        {
            CartItems = DataStore.GetCartItems();
            decimal totalInCart = 0.00M;
            ProductsInOrder = new List<ProductInOrder>();
            foreach (var item in CartItems)
            {
                totalInCart += item.Cantitate * item.Price;
                ProductsInOrder.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
            }
            Order = new Order
            {
                CustomerId = App.userInfo?.Email,
                Status = "Ordered",
                OrderId = 0,
                TotalOrdered = totalInCart
            };
            OrderInfo = new OrderInfo
            {
                FullName = App.userInfo?.FullName,
                OrderInfoId = 0,
                PhoneNo = App.userInfo?.PhoneNumber,
                Address = String.Concat(App.userInfo?.BuildingInfo, ", ", App.userInfo?.Street, ", ", App.userInfo?.City)
            };
            PlaceFinalOrder = new Command(OnClickPlaceOrder);
        }

        async void OnClickPlaceOrder()
        {
            var result = await OrderService.CreateOrder(Order);
            if (result > 0)
            {
                OrderInfo.OrderRefId = result;
                await OrderService.CreateOrderInfo(OrderInfo);
                foreach (var item in ProductsInOrder)
                {
                    item.OrderRefId = result;
                }
                await OrderService.CreateProductsInOrder(ProductsInOrder);
            }
            DataStore.CleanCart();
            OnPlaceOrder?.Invoke(this, default(EventArgs));
        }
    }
}
