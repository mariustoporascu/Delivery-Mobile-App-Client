using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace FoodDeliveryApp.ViewModels
{
    public class ListaRestauranteViewModel : BaseViewModel
    {
        private Companie _selectedItem;
        private ObservableRangeCollection<Companie> _items;
        public ObservableRangeCollection<Companie> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command LoadItemsCommand { get; }
        public Command RefreshCommand { get; }
        public Command<Companie> ItemTapped { get; }

        public ListaRestauranteViewModel()
        {
            Title = "Lista Restaurante";
            Items = new ObservableRangeCollection<Companie>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            RefreshCommand = new Command(async () => await RefreshItems());
            ItemTapped = new Command<Companie>(async (item) => await OnItemSelected(item));
        }

        void ExecuteLoadItemsCommand()
        {
            try
            {

                Items.Clear();
                var items = DataStore.GetRestaurante();

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
                var items = DataStore.GetRestaurante().ToList();
                if (items.Count != Items.Count)
                    ExecuteLoadItemsCommand();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }

        public Companie SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        async Task OnItemSelected(Companie item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(CategViewModel.Canal)}=2&{nameof(CategViewModel.RefId)}={item.RestaurantId}");
        }
    }
}