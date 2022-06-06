using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class EntryFoodAppViewModel : BaseViewModel
    {
        public Command<TipCompanie> ItemTapped { get; }
        private TipCompanie _selectedItem;
        private ObservableRangeCollection<TipCompanie> _items;
        public ObservableRangeCollection<TipCompanie> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command LoadItemsCommand { get; }
        public Command RefreshCommand { get; }
        public event EventHandler NotOpen = delegate { };

        public EntryFoodAppViewModel()
        {
            Title = "Acasa";
            Items = new ObservableRangeCollection<TipCompanie>();
            ItemTapped = new Command<TipCompanie>((item) => OnItemSelected(item));
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            RefreshCommand = new Command(async () => await RefreshItems());
        }
        void ExecuteLoadItemsCommand()
        {
            try
            {

                Items.Clear();
                var items = DataStore.GetTipCompanii();

                Items.AddRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }
        public async Task RefreshItems()
        {
            IsBusy = true;
            try
            {
                await ReloadServerData();
                var items = DataStore.GetTipCompanii().ToList();
                if (items.Count != Items.Count)
                    ExecuteLoadItemsCommand();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }
        public TipCompanie SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        async void OnItemSelected(TipCompanie item)
        {
            if (item == null)
                return;
            if (!item.IsOpen)
            {
                NotOpen?.Invoke(this, new EventArgs());
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(ListaRestaurantePage)}?{nameof(ListaRestauranteViewModel.TipId)}={item.TipCompanieId}");
        }
    }
}
