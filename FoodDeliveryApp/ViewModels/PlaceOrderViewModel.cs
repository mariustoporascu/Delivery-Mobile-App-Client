using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {
        private bool hasValidProfile = false;
        public bool HasValidProfile { get => hasValidProfile; set => SetProperty(ref hasValidProfile, value); }
        public Order OrderMarket { get; set; }
        public Dictionary<string, Order> OrderRestaurant { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderMarket { get; set; }
        public Dictionary<string, List<ProductInOrder>> ProductsInOrderRestaurant { get; set; }
        private Dictionary<string, decimal> totalRestaurant;
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };

        private decimal totalSuperMarket;
        public PlaceOrderViewModel()
        {
            HasValidProfile = App.userInfo.CompleteProfile && App.userInfo.CompleteLocation;
            LoadPageData();
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
        }
        void LoadPageData()
        {
            CartItems = DataStore.GetCartItems();
            totalSuperMarket = 0.00M;
            totalRestaurant = new Dictionary<string, decimal>();
            ProductsInOrderMarket = new List<ProductInOrder>();
            OrderRestaurant = new Dictionary<string, Order>();
            ProductsInOrderRestaurant = new Dictionary<string, List<ProductInOrder>>();
            foreach (var item in CartItems)
            {
                if (item.Canal == 1)
                {
                    totalSuperMarket += item.Cantitate * item.Price;
                    ProductsInOrderMarket.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
                }
                else
                {
                    if (!totalRestaurant.ContainsKey(((int)item.ShopId).ToString()))
                        totalRestaurant.Add(((int)item.ShopId).ToString(), item.Cantitate * item.Price);
                    else
                        totalRestaurant[((int)item.ShopId).ToString()] += item.Cantitate * item.Price;
                    if (!ProductsInOrderRestaurant.ContainsKey(((int)item.ShopId).ToString()))
                    {
                        ProductsInOrderRestaurant.Add(((int)item.ShopId).ToString(), new List<ProductInOrder>());
                        ProductsInOrderRestaurant[((int)item.ShopId).ToString()].Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
                    }
                    else
                        ProductsInOrderRestaurant[((int)item.ShopId).ToString()].Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate });
                }

            }
            if (ProductsInOrderMarket.Count > 0)
            {
                OrderMarket = new Order
                {
                    CustomerId = App.userInfo.Email,
                    Status = "Ordered",
                    OrderId = 0,
                    Created = DateTime.UtcNow.AddHours(3),
                    TotalOrdered = totalSuperMarket
                };
            }
            if (ProductsInOrderRestaurant.Count > 0)
            {
                foreach (var total in totalRestaurant)
                    OrderRestaurant.Add(total.Key, new Order
                    {
                        CustomerId = App.userInfo.Email,
                        Status = "Ordered",
                        OrderId = 0,
                        Created = DateTime.UtcNow.AddHours(3),
                        IsRestaurant = true,
                        RestaurantRefId = Int32.Parse(total.Key),
                        NumeCompanie = DataStore.GetRestaurant(Int32.Parse(total.Key)).Name,
                        TotalOrdered = total.Value,
                        TransportFee = total.Value >= DataStore.GetRestaurant(Int32.Parse(total.Key)).MinimumOrderValue
                                ? 0 : DataStore.GetRestaurant(Int32.Parse(total.Key)).TransporFee,
                    });
            }
            OrderInfo = new OrderInfo
            {
                FullName = App.userInfo.FullName,
                OrderInfoId = 0,
                PhoneNo = App.userInfo.PhoneNumber,
                Address = String.Concat(App.userInfo.BuildingInfo, ", ", App.userInfo.Street, ", ", App.userInfo.City)
            };

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
                foreach (var products in ProductsInOrderRestaurant)
                {
                    var result = await OrderService.CreateOrder(OrderRestaurant[products.Key]);
                    if (result > 0)
                    {
                        OrderInfo.OrderRefId = result;
                        await OrderService.CreateOrderInfo(OrderInfo);
                        foreach (var item in products.Value)
                        {
                            item.OrderRefId = result;
                        }
                        await OrderService.CreateProductsInOrder(products.Value);
                    }
                }

            }
            DataStore.CleanCart();
            OnPlaceOrder?.Invoke(this, new EventArgs());
        }
    }
}
