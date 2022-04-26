using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class ListaRestauranteViewModel : BaseViewModel
    {
        private Companie _selectedItem;
        private ObservableCollection<Companie> _items;
        public ObservableCollection<Companie> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command LoadItemsCommand { get; }
        public Command<Companie> ItemTapped { get; }

        public ListaRestauranteViewModel()
        {
            Title = "Lista Restaurante";
            Items = new ObservableCollection<Companie>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Companie>(async (item) => await OnItemSelected(item));
        }

        void ExecuteLoadItemsCommand()
        {

            try
            {
                Items.Clear();
                var items = DataStore.GetRestaurante();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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