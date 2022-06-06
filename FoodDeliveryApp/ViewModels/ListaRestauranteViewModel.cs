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
    [QueryProperty(nameof(TipId), nameof(TipId))]
    public class ListaRestauranteViewModel : BaseViewModel
    {
        private Companie _selectedItem;
        public event EventHandler NotOpen = delegate { };

        private ObservableRangeCollection<Companie> _items;
        public ObservableRangeCollection<Companie> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        private int tipId;
        public int TipId
        {
            get
            {
                return tipId;
            }
            set
            {
                tipId = value;
            }
        }
        public Command LoadItemsCommand { get; }
        public Command RefreshCommand { get; }
        public Command<Companie> ItemTapped { get; }

        public ListaRestauranteViewModel()
        {
            Items = new ObservableRangeCollection<Companie>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            RefreshCommand = new Command(async () => await RefreshItems());
            ItemTapped = new Command<Companie>((item) => OnItemSelected(item));
        }

        void ExecuteLoadItemsCommand()
        {
            try
            {

                Items.Clear();
                var items = DataStore.GetCompanii(TipId);
                Title = "Lista " + DataStore.GetTipCompanii().First(tip => tip.TipCompanieId == TipId).Name;

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
                var items = DataStore.GetCompanii(TipId).ToList();
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
        async void OnItemSelected(Companie item)
        {
            if (item == null)
                return;
            if (!item.IsActive)
            {
                NotOpen?.Invoke(this, new EventArgs());
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(CategViewModel.RefId)}={item.CompanieId}");
        }
    }
}