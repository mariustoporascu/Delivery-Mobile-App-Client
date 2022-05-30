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
        public ServerOrder OrderMarket { get; set; }
        public Dictionary<string, ServerOrder> OrderRestaurant { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderMarket { get; set; }
        public Dictionary<string, List<ProductInOrder>> ProductsInOrderRestaurant { get; set; }
        private Dictionary<string, decimal> totalRestaurant;
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };
        public event EventHandler OnPlaceOrderMarket = delegate { };
        public event EventHandler OnPlaceOrderFailed = delegate { };

        private decimal totalSuperMarket;
        public PlaceOrderViewModel()
        {
            HasValidProfile = App.UserInfo.CompleteProfile && App.UserInfo.Location != null;
            if (HasValidProfile)
                LoadPageData();
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
        }
        void LoadPageData()
        {
            CartItems = DataStore.GetCartItems();
            totalSuperMarket = 0.00M;
            totalRestaurant = new Dictionary<string, decimal>();
            ProductsInOrderMarket = new List<ProductInOrder>();
            OrderRestaurant = new Dictionary<string, ServerOrder>();
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
                OrderMarket = new ServerOrder
                {
                    CustomerId = App.UserInfo.Email,
                    Status = "Ordered",
                    OrderId = 0,
                    Created = DateTime.UtcNow.AddHours(3),
                    TotalOrdered = totalSuperMarket
                };
            }
            if (ProductsInOrderRestaurant.Count > 0)
            {
                foreach (var total in totalRestaurant)
                    OrderRestaurant.Add(total.Key, new ServerOrder
                    {
                        CustomerId = App.UserInfo.Email,
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
                FullName = App.UserInfo.FullName,
                OrderInfoId = 0,
                PhoneNo = App.UserInfo.PhoneNumber,
                Address = String.Concat(App.UserInfo.Location.BuildingInfo, ", ", App.UserInfo.Location.Street, ", ", App.UserInfo.Location.City)
            };
            if (OrderMarket != null)
            {
                OrderMarket.ProductsInOrder = ProductsInOrderMarket;
                OrderMarket.OrderInfos = OrderInfo;
            }

            foreach (var order in OrderRestaurant)
            {
                order.Value.OrderInfos = OrderInfo;
                order.Value.ProductsInOrder = ProductsInOrderRestaurant[order.Key];
            }
        }

        async Task OnClickPlaceOrder()
        {
            bool marketOrderOk = false;
            List<bool> restaurantOrdersplaced = new List<bool>();
            if (ProductsInOrderMarket.Count > 0)
            {
                var result = await OrderService.CreateOrder(OrderMarket);
                if (!string.IsNullOrEmpty(result) && result.Contains("Order placed."))
                {
                    marketOrderOk = true;
                }
            }
            if (ProductsInOrderMarket.Count > 0 && marketOrderOk)
            {
                OnPlaceOrderMarket?.Invoke(this, new EventArgs());
            }
            if (ProductsInOrderRestaurant.Count > 0)
            {
                var iterator = 0;
                foreach (var order in OrderRestaurant)
                {
                    var result = await OrderService.CreateOrder(OrderRestaurant[order.Key]);
                    if (!string.IsNullOrEmpty(result) && result.Contains("Order placed."))
                    {
                        restaurantOrdersplaced.Add(true);
                    }
                    else
                        restaurantOrdersplaced.Add(false);
                }

            }
            if (ProductsInOrderRestaurant.Count > 0 && restaurantOrdersplaced.FindAll(item => item == false).Count == 0)
            {
                OnPlaceOrder?.Invoke(this, new EventArgs());
                DataStore.CleanCart();
            }
            else
                OnPlaceOrderFailed?.Invoke(this, new EventArgs());

        }
    }
}
