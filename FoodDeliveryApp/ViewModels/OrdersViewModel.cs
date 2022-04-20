using FoodDeliveryApp.Models.ShopModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private ObservableRangeCollection<ServerOrder> _orders;
        public ObservableRangeCollection<ServerOrder> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        public OrdersViewModel()
        {
            Orders = new ObservableRangeCollection<ServerOrder>();
            IsLoggedIn = App.isLoggedIn;

            LoadOrdersCommand = new Command(ExecuteLoadOrdersCommand);
        }
        public void ExecuteLoadOrdersCommand()
        {
            IsBusy = true;
            string email = App.userInfo?.Email;
            try
            {
                Orders.Clear();
                var serverOrders = DataStore.GetServerOrders(email);
                if (serverOrders != null)
                    Orders.AddRange(serverOrders);
                if (Orders.Count > 0)
                {
                    IsPageVisible = true;
                }
                else
                    IsPageVisible = false;
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
    }
}
