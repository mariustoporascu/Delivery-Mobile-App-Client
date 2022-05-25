using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(Canal), nameof(Canal))]
    [QueryProperty(nameof(RefId), nameof(RefId))]
    public class CategViewModel : BaseViewModel
    {
        private Categ _selectedItem;
        private int canal;
        private int refId;
        private ObservableRangeCollection<Categ> _items;
        public ObservableRangeCollection<Categ> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command LoadItemsCommand { get; }
        public Command<Categ> ItemTapped { get; }
        public Command AllProductsTapped { get; }

        public CategViewModel()
        {
            Title = "Categorii";
            Items = new ObservableRangeCollection<Categ>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Categ>(async (item) => await OnItemSelected(item));
            AllProductsTapped = new Command(async () => await AllProducts());
        }
        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var items = DataStore.GetCategories(canal, refId);

                Items.AddRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }
        public int Canal
        {
            get
            {
                return canal;
            }
            set
            {
                canal = value;
            }
        }
        public int RefId
        {
            get
            {
                return refId;
            }
            set
            {
                refId = value;
            }
        }
        public Categ SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        async Task OnItemSelected(Categ item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ItemsPage)}?{nameof(ItemsViewModel.Canal)}={canal}&{nameof(ItemsViewModel.RefId)}={refId}&{nameof(ItemsViewModel.CategId)}={item.CategoryId}");

        }
        async Task AllProducts()
        {
            await Shell.Current.GoToAsync($"{nameof(ItemsPage)}?{nameof(ItemsViewModel.Canal)}={canal}&{nameof(ItemsViewModel.RefId)}={refId}&{nameof(ItemsViewModel.CategId)}=0");
        }
    }
}