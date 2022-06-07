using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {

        private bool hasValidProfile = false;
        public bool HasValidProfile { get => hasValidProfile; set => SetProperty(ref hasValidProfile, value); }
        public ServerOrder OrderCompanie { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderCompanie { get; set; }
        private decimal totalCompanie = 0.00M;
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };
        public event EventHandler OnPlaceOrderFailed = delegate { };
        private int locationId;
        private string paymentMethod;
        public int LocationId { get => locationId; set => SetProperty(ref locationId, value); }
        public string PaymentMethod { get => paymentMethod; set => SetProperty(ref paymentMethod, value); }
        public PlaceOrderViewModel(int locationId, string paymentMethod)
        {
            HasValidProfile = App.UserInfo.CompleteProfile && (App.UserInfo.Locations != null && App.UserInfo.Locations.Count > 0);
            LocationId = locationId;
            PaymentMethod = paymentMethod;
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
            if (HasValidProfile)
                LoadPageData();
        }

        public void LoadPageData()
        {
            CartItems = DataStore.GetCartItems();
            ProductsInOrderCompanie = new List<ProductInOrder>();
            foreach (var item in CartItems)
            {
                totalCompanie += item.PriceTotal;
                ProductsInOrderCompanie.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate, ClientComments = item.ClientComments });

            }

            if (ProductsInOrderCompanie.Count > 0)
            {
                OrderCompanie = new ServerOrder
                {
                    CustomerId = App.UserInfo.Email,
                    Status = "Plasata",
                    OrderId = 0,
                    Created = DateTime.UtcNow.AddHours(3),
                    CompanieRefId = CartItems[0].CompanieRefId,
                    TelephoneOrdered = false,
                    UserLocationId = LocationId,
                    PaymentMethod = PaymentMethod,
                    NumeCompanie = DataStore.GetCompanie(CartItems[0].CompanieRefId).Name,
                    TotalOrdered = totalCompanie,
                    TransportFee = totalCompanie >= DataStore.GetCompanie(CartItems[0].CompanieRefId)
                        .TransportFees.Find(fee => fee.CityRefId == DataStore.GetAvailableCities().ToList()
                        .Find(city => city.Name == App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId).City).CityId)
                        .MinimumOrderValue ? 0 : DataStore.GetCompanie(CartItems[0].CompanieRefId)
                        .TransportFees.Find(fee => fee.CityRefId == DataStore.GetAvailableCities().ToList()
                        .Find(city => city.Name == App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId).City).CityId).TransporFee,
                };
            }
            if (OrderCompanie != null)
            {
                OrderInfo = new OrderInfo
                {
                    FullName = App.UserInfo.FullName,
                    OrderInfoId = 0,
                    PhoneNo = App.UserInfo.PhoneNumber,
                    Address = String.Concat(App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId).BuildingInfo, ", ", App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId).Street, ", ", App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId).City)
                };

                OrderCompanie.OrderInfos = OrderInfo;
                OrderCompanie.ProductsInOrder = ProductsInOrderCompanie;
            }


        }

        async Task OnClickPlaceOrder()
        {

            var result = await OrderService.CreateOrder(OrderCompanie);
            if (!string.IsNullOrEmpty(result) && result.Contains("Order placed."))
            {
                DataStore.CleanCart();
                OnPlaceOrder?.Invoke(this, new EventArgs());
            }
            else
                OnPlaceOrderFailed?.Invoke(this, new EventArgs());

        }
    }
}
