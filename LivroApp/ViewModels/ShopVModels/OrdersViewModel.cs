using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class OrdersViewModel : BaseViewModel<Order>
    {
        public DateTime SelectedTime { get; set; } = DateTime.UtcNow.AddHours(3);
        List<Order> uiOrders;
        private bool isLoading = false;

        public OrdersViewModel()
        {
            uiOrders = new List<Order>();
            Items = new ObservableRangeCollection<Order>();
            ItemTapped = new Command<Order>(OnItemSelected);
            LoadAllItems = new Command(async () => await ExecuteLoadOrdersCommand());
        }
        public async Task ExecuteLoadOrdersCommand()
        {
            if (!isLoading)
            {
                isLoading = true;
                IsBusy = true;
                string email = App.UserInfo.Email;
                try
                {
                    uiOrders.Clear();
                    var serverOrders = await DataStore.GetServerOrders(email);

                    foreach (ServerOrder serverOrder in serverOrders)
                        uiOrders.Add(new Order
                        {
                            OrderId = serverOrder.OrderId,
                            Status = serverOrder.Status,
                            TotalOrdered = serverOrder.TotalOrdered + serverOrder.TransportFee,
                            Created = serverOrder.Created,
                        });
                    FilterBy(SelectedTime);
                    if (Items.Count > 0)
                    {
                        IsAvailable = true;
                    }
                    else
                        IsAvailable = false;
                    await Task.Delay(1000);

                }
                catch (Exception) { }
                finally
                {
                    IsBusy = false;
                    isLoading = false;
                }
            }

        }
        public void FilterBy(DateTime time)
        {
            Items.Clear();
            if (uiOrders != null && uiOrders.Count > 0)
                Items.AddRange(uiOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month));

        }
        async void OnItemSelected(Order item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderInfoPage)}?{nameof(OrderInfoViewModel.OrderId)}={item.OrderId}");
        }
    }
}
