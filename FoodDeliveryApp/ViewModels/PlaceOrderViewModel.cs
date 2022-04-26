using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {
        public Order OrderMarket { get; set; }
        public Order OrderRestaurant { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderMarket { get; set; }
        public List<ProductInOrder> ProductsInOrderRestaurant { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };
        private bool hasBothTypes = false;
        public bool HasBothTypes { get => hasBothTypes; set => SetProperty(ref hasBothTypes, value); }
        private bool hasValidProfile = false;
        public bool HasValidProfile { get => hasValidProfile; set => SetProperty(ref hasValidProfile, value); }
        public PlaceOrderViewModel()
        {
            HasValidProfile = App.userInfo.CompleteProfile;
            LoadPageData();
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
        }
        void LoadPageData()
        {
            CartItems = DataStore.GetCartItems();
            decimal totalInCart1 = 0.00M;
            decimal totalInCart2 = 0.00M;
            ProductsInOrderMarket = new List<ProductInOrder>();
            ProductsInOrderRestaurant = new List<ProductInOrder>();
            foreach (var item in CartItems)
            {
                if (item.Canal == 1)
                {
                    totalInCart1 += item.Cantitate * item.Price;
                    ProductsInOrderMarket.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
                }
                else
                {
                    totalInCart2 += item.Cantitate * item.Price;
                    ProductsInOrderRestaurant.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
                }

            }
            if (ProductsInOrderMarket.Count > 0)
            {
                OrderMarket = new Order
                {
                    CustomerId = App.userInfo.Email,
                    Status = "Ordered",
                    OrderId = 0,
                    TotalOrdered = totalInCart1
                };
            }
            if (ProductsInOrderRestaurant.Count > 0)
            {
                OrderRestaurant = new Order
                {
                    CustomerId = App.userInfo.Email,
                    Status = "Ordered",
                    OrderId = 0,
                    TotalOrdered = totalInCart2
                };
            }
            OrderInfo = new OrderInfo
            {
                FullName = App.userInfo.FullName,
                OrderInfoId = 0,
                PhoneNo = App.userInfo.PhoneNumber,
                Address = String.Concat(App.userInfo.BuildingInfo, ", ", App.userInfo.Street, ", ", App.userInfo.City)
            };
            if (ProductsInOrderMarket.Count > 0 && ProductsInOrderRestaurant.Count > 0)
                HasBothTypes = true;
        }

        async Task OnClickPlaceOrder()
        {
            if (ProductsInOrderMarket.Count > 0)
            {
                var result = await OrderService.CreateOrder(OrderMarket);
                if (result > 0)
                {
                    OrderInfo.OrderRefId = result;
                    await OrderService.CreateOrderInfo(OrderInfo);
                    foreach (var item in ProductsInOrderMarket)
                    {
                        item.OrderRefId = result;
                    }
                    await OrderService.CreateProductsInOrder(ProductsInOrderMarket);
                }
            }

            if (ProductsInOrderRestaurant.Count > 0)
            {
                var result = await OrderService.CreateOrder(OrderRestaurant);
                if (result > 0)
                {
                    OrderInfo.OrderRefId = result;
                    await OrderService.CreateOrderInfo(OrderInfo);
                    foreach (var item in ProductsInOrderRestaurant)
                    {
                        item.OrderRefId = result;
                    }
                    await OrderService.CreateProductsInOrder(ProductsInOrderRestaurant);
                }
            }
            DataStore.CleanCart();
            OnPlaceOrder?.Invoke(this, new EventArgs());
        }
    }
}
