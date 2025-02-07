﻿using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public class OrderInfoViewModel : BaseViewModel<OrderProductDisplay>
    {
        private OrderInfo orderInfo;
        public OrderInfo CurrOrderInfo { get => orderInfo; set => SetProperty(ref orderInfo, value); }

        private Order order;
        public Order CurrOrder { get => order; set => SetProperty(ref order, value); }
        private Companie _restaurant;
        public Companie Restaurant { get => _restaurant; set => SetProperty(ref _restaurant, value); }
        private Driver orderDriver;
        public Driver CurrOrderDriver { get => orderDriver; set => SetProperty(ref orderDriver, value); }
        private bool _hasDriver = false;
        public bool HasDriver { get => _hasDriver; set => SetProperty(ref _hasDriver, value); }
        private bool _ownerViewVis = false;
        public bool OwnerViewVis { get => _ownerViewVis; set => SetProperty(ref _ownerViewVis, value); }
        private bool _hasEstimatedTime = false;
        public bool HasEstimatedTime { get => _hasEstimatedTime; set => SetProperty(ref _hasEstimatedTime, value); }
        private bool _hasUserResponded = false;
        public bool HasUserResponded { get => _hasUserResponded; set => SetProperty(ref _hasUserResponded, value); }
        private bool _canGiveRating = false;
        public bool CanGiveRating { get => _canGiveRating; set => SetProperty(ref _canGiveRating, value); }
        private bool _hasComments = false;
        public bool HasComments { get => _hasComments; set => SetProperty(ref _hasComments, value); }
        public Command RefreshCommand { get; }
        public Command ChangeRatingDriver { get; }
        public Command ChangeRatingRestaurant { get; }
        public event EventHandler GetRatDriver = delegate { };
        public event EventHandler GetRatRest = delegate { };


        public OrderInfoViewModel()
        {
            Items = new ObservableRangeCollection<OrderProductDisplay>();
            ItemTapped = new Command<OrderProductDisplay>(async (item) => await OnItemSelected(item));
            RefreshCommand = new Command(RefreshView);
            ChangeRatingDriver = new Command(IntermediatDriverRating);
            ChangeRatingRestaurant = new Command(IntermediateRestRating);
        }
        private int _orderId;
        public int OrderId { get => _orderId; set { _orderId = value; LoadOrder(value); } }

        public async Task<bool> ConfirmOrder(bool value)
        {
            try
            {
                if (await OrderService.AgreeEstTime(OrderId, value))
                {
                    CurrOrder.HasUserConfirmedET = value;
                    HasUserResponded = true;
                    return true;
                }
                HasUserResponded = false;
                return false;
            }
            catch (Exception) { return false; }

        }
        public async void RefreshView()
        {
            IsBusy = true;
            try
            {
                if (OrderId > 0)
                {
                    var serverOrders = await DataStore.GetServerOrders(App.UserInfo.Email);

                    if (serverOrders.Count > 0)
                        LoadOrder(OrderId);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load order");
            }
            IsBusy = false;
        }
        public void LoadOrder(int orderId)
        {
            try
            {
                var order = DataStore.GetOrder(orderId);
                Title = "Detalii Comanda nr. " + orderId;
                CurrOrderInfo = order.OrderInfos;
                CurrOrder = new Order
                {
                    Status = order.Status,
                    TotalOrdered = order.TotalOrdered,
                    TotalOrderedInterfata = order.TotalOrdered + " RON",
                    TransportFee = order.TransportFee,
                    Comments = order.Comments,
                    EstimatedTime = order.EstimatedTime,
                    PaymentMethod = order.PaymentMethod,
                    HasUserConfirmedET = order.HasUserConfirmedET,
                    ClientGaveRatingDriver = order.ClientGaveRatingDriver,
                    ClientGaveRatingCompanie = order.ClientGaveRatingCompanie,
                    RatingDriver = order.RatingDriver,
                    RatingCompanie = order.RatingCompanie,
                    DriverRefId = order.DriverRefId,
                };
                if (CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                    CanGiveRating = true;
                else
                    CanGiveRating = false;
                Items.Clear();
                if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                {
                    HasDriver = true;
                    CurrOrderDriver = order.OrderDriver;
                }
                else
                {
                    HasDriver = false;
                }
                Restaurant = DataStore.GetCompanie((int)order.CompanieRefId);
                OwnerViewVis = true;
                if (order.HasUserConfirmedET != null)
                    HasUserResponded = true;
                else
                    HasUserResponded = false;
                if (order.EstimatedTime != null)
                    HasEstimatedTime = true;
                else
                    HasEstimatedTime = false;
                HasComments = !string.IsNullOrWhiteSpace(order.Comments);
                var itemsInOrder = new ObservableRangeCollection<OrderProductDisplay>();
                foreach (var prodInOrder in order.ProductsInOrder)
                {
                    var item = DataStore.GetItem(prodInOrder.ProductRefId);

                    itemsInOrder.Add(new OrderProductDisplay
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        GramajInterfata = item.GramajInterfata,
                        PretInterfata = (item.Price * prodInOrder.UsedQuantity).ToString() + " RON",
                        Cantitate = prodInOrder.UsedQuantity,
                        ClientComments = prodInOrder.ClientComments,
                        HasComments = !string.IsNullOrWhiteSpace(prodInOrder.ClientComments)
                    });
                }
                Items.AddRange(itemsInOrder);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load order");
            }
        }
        public void IntermediatDriverRating()
        {
            GetRatDriver?.Invoke(this, new EventArgs());
        }
        public async Task<bool> GiveDriverRating(int rating)
        {
            try
            {
                if (await OrderService.GiveRatingDriver(App.UserInfo.Email, OrderId, rating))
                {
                    CurrOrder.ClientGaveRatingDriver = true;
                    CurrOrder.RatingDriver = rating;
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }
        public void IntermediateRestRating()
        {
            GetRatRest?.Invoke(this, new EventArgs());
        }
        public async Task<bool> GiveRestaurantRating(int rating)
        {
            try
            {
                if (await OrderService.GiveRatingRestaurant(App.UserInfo.Email, OrderId, rating))
                {
                    CurrOrder.ClientGaveRatingCompanie = true;
                    CurrOrder.RatingCompanie = rating;
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }
        async Task OnItemSelected(OrderProductDisplay item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?{nameof(ProductDetailViewModel.ItemId)}={item.ProductId}");
        }
    }
}
