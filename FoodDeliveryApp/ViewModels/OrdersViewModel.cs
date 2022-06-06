using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Order> _orders;
        public ObservableRangeCollection<Order> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public DateTime SelectedTime { get; set; } = DateTime.UtcNow.AddHours(3);
        List<Order> uiOrders;
        public Command<Order> ItemTapped { get; }

        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        public OrdersViewModel()
        {
            Orders = new ObservableRangeCollection<Order>();
            IsLoggedIn = App.IsLoggedIn;
            ItemTapped = new Command<Order>(OnItemSelected);

            LoadOrdersCommand = new Command(ExecuteLoadOrdersCommand);
        }
        public async void ExecuteLoadOrdersCommand()
        {
            IsBusy = true;
            string email = App.UserInfo.Email;
            try
            {
                uiOrders = new List<Order>();
                var serverOrders = await DataStore.GetServerOrders(email);

                /*if (Device.RuntimePlatform == Device.Android)
                    serverOrders = await DataStore.GetServerOrders(email);
                else
                    serverOrders = DataStore.GetServerOrders(email).GetAwaiter().GetResult();*/

                lock (Orders)
                {
                    foreach (ServerOrder serverOrder in serverOrders)
                        uiOrders.Add(new Order
                        {
                            OrderId = serverOrder.OrderId,
                            Status = serverOrder.Status,
                            TotalOrdered = serverOrder.TotalOrdered + serverOrder.TransportFee,
                            Created = serverOrder.Created,
                        });
                    FilterBy(SelectedTime);
                    if (Orders.Count > 0)
                    {
                        IsPageVisible = true;
                    }
                    else
                        IsPageVisible = false;
                }
                await Task.Delay(1000);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void FilterBy(DateTime time)
        {
            Orders.Clear();
            if (uiOrders != null)
                Orders.AddRange(uiOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month));

        }
        async void OnItemSelected(Order item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderInfoPage)}?{nameof(OrderInfoViewModel.OrderId)}={item.OrderId}");
        }
    }
}
