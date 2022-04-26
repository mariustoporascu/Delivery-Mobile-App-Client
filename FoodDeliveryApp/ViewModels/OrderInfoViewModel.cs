using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    internal class OrderInfoViewModel : BaseViewModel
    {
        private OrderInfo orderInfo;
        private Order order;
        public Order CurrOrder { get => order; set => SetProperty(ref order, value); }
        public OrderInfo CurrOrderInfo { get => orderInfo; set => SetProperty(ref orderInfo, value); }
        private ObservableRangeCollection<OrderProductDisplay> _items;
        public ObservableRangeCollection<OrderProductDisplay> Items { get => _items; set => SetProperty(ref _items, value); }
        public Command<OrderProductDisplay> ItemTapped { get; }

        private int orderId;

        public OrderInfoViewModel()
        {
            Title = "Detalii Comanda";
            Items = new ObservableRangeCollection<OrderProductDisplay>();
            ItemTapped = new Command<OrderProductDisplay>(async (item) => await OnItemSelected(item));
        }
        public int OrderId
        {
            get
            {
                return orderId;
            }
            set
            {
                orderId = value;
                LoadOrder(value);
            }
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
                    TotalOrderedInterfata = order.TotalOrdered + " RON"
                };
                Items.Clear();
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
                        Cantitate = prodInOrder.UsedQuantity
                    });
                }
                Items.AddRange(itemsInOrder);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load order");
            }
        }
        async Task OnItemSelected(OrderProductDisplay item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ProductInOrderPage)}?{nameof(ProductInOrderViewModel.ItemId)}={item.ProductId}");
        }
    }
}
