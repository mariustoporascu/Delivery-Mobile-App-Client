using LivroApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class PlaceOrderViewModel : BaseViewModel<object>
    {
        private decimal totalCompanie = 0.00M;
        public ServerOrder OrderCompanie { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderCompanie { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        private int locationId;
        public int LocationId { get => locationId; set => SetProperty(ref locationId, value); }
        private string paymentMethod;
        public string PaymentMethod { get => paymentMethod; set => SetProperty(ref paymentMethod, value); }
        public PlaceOrderViewModel(int locationId, string paymentMethod)
        {
            IsAvailable = App.UserInfo.CompleteProfile && App.UserInfo.Locations != null && App.UserInfo.Locations.Count > 0;
            LocationId = locationId;
            PaymentMethod = paymentMethod;
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
            if (IsAvailable)
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
                var transportFeeObj = DataStore.GetCompanie(CartItems[0].CompanieRefId)
                        .TransportFees.Find(fee => fee.CityRefId == DataStore.GetAvailableCities().ToList()
                        .Find(city => city.Name == App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId)?.City)?.CityId);
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
                    // 0 or if not null transport fee value
                    TransportFee = totalCompanie >= transportFeeObj?.MinimumOrderValue ? 0 : transportFeeObj?.TransporFee ?? 0,

                };
            }
            if (OrderCompanie != null)
            {
                var userLoc = App.UserInfo.Locations.Find(loc => loc.LocationId == LocationId);
                OrderInfo = new OrderInfo
                {
                    FullName = App.UserInfo.FullName,
                    OrderInfoId = 0,
                    PhoneNo = App.UserInfo.PhoneNumber,
                    Address = string.Concat(userLoc?.BuildingInfo, ", ", userLoc?.Street, ", ", userLoc?.City)
                };

                OrderCompanie.OrderInfos = OrderInfo;
                OrderCompanie.ProductsInOrder = ProductsInOrderCompanie;
            }
        }

        async Task OnClickPlaceOrder()
        {
            IsBusy = true;
            try
            {
                var result = await OrderService.CreateOrder(OrderCompanie);
                if (!string.IsNullOrEmpty(result) && result.Contains("Order placed."))
                {
                    DataStore.CleanCart();
                    CallSuccessEvent();
                }
                else
                    CallFailedEvent();
            }
            catch (Exception) { CallFailedEvent(); }
            finally { IsBusy = false; }
        }
    }
}
