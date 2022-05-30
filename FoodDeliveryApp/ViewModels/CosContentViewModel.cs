using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class CosContentViewModel : BaseViewModel
    {
        private CartItem _selectedItem;
        private decimal _total;
        private ObservableCollection<CartItem> _items;
        public ObservableCollection<CartItem> Items { get => _items; set => SetProperty(ref _items, value); }
        private bool isPageVisible = false;
        HttpClient _client;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        private bool isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set => SetProperty(ref isLoggedIn, value);
        }
        private bool canPlaceOrder = false;
        public bool CanPlaceOrder
        {
            get => canPlaceOrder;
            set => SetProperty(ref canPlaceOrder, value);
        }
        public Command LoadItemsCommand { get; }
        public Command MinusCommand { get; }
        public Command PlusCommand { get; }
        public Command DeleteCommand { get; }
        public Command<CartItem> ItemTapped { get; }

        public CosContentViewModel()
        {
            Title = "Cos cumparaturi";
            Items = new ObservableCollection<CartItem>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            _client = new HttpClient();

            ItemTapped = new Command<CartItem>((item) => OnItemSelected(item));

            MinusCommand = new Command<CartItem>(OnMinus);
            PlusCommand = new Command<CartItem>(OnPlus);
            DeleteCommand = new Command<CartItem>(OnDelete);
        }
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        void ExecuteLoadItemsCommand()
        {

            try
            {
                Items.Clear();
                Total = 0;
                var items = DataStore.GetCartItems();
                foreach (var item in items)
                {
                    Items.Add(item);
                    Total = Total + item.Cantitate * item.Price;
                }
                if (items.Count > 0)
                    IsPageVisible = true;
                else
                    IsPageVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public CartItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        void OnDelete(CartItem item)
        {
            Items.Remove(item);
            DataStore.DeleteFromCart(item);
            RefreshCanExecutes();

        }
        void OnMinus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate--;
            if (item.Cantitate == 0)
            {
                DataStore.DeleteFromCart(item);
                Items.Remove(item);
            }
            else
                DataStore.SaveCart(item);

            RefreshCanExecutes();
        }
        void OnPlus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate++;
            DataStore.SaveCart(item);
            RefreshCanExecutes();
        }
        void RefreshCanExecutes()
        {
            Total = 0;
            foreach (var item in Items)
            {
                Total = Total + item.Cantitate * item.Price;
            }
            if (Items.Count > 0)
                IsPageVisible = true;
            else
                IsPageVisible = false;

        }
        async void OnItemSelected(CartItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.ProductId}");
        }
        public async void GetTime()
        {
            Uri uri = new Uri(ServerConstants.TimeUrl);
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var timeObject = JsonConvert.DeserializeObject<WorldTime>(content, settings);
                CanPlaceOrder = timeObject.DateTime.Hour >= 9 && timeObject.DateTime.Hour < 23;
            }
            else { CanPlaceOrder = false; }
        }
        partial class WorldTime
        {
            public DateTime DateTime { get; set; }
        }
    }
}
