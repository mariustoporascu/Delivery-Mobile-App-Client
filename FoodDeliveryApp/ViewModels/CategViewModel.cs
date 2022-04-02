using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
            ItemTapped = new Command<Categ>(OnItemSelected);
            AllProductsTapped = new Command(AllProducts);
        }

        private ImageSource GetSource(Categ item)
        {
            if (item.Image != null)
            {
                byte[] bytes = Convert.FromBase64String(item.Image);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return ImageSource.FromFile("No_image_available.png");
        }
        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var newItems = new ObservableRangeCollection<Categ>();
                var items = DataStore.GetCategories(canal, refId);
                foreach (var item in items)
                {
                    if (item.ImageFinal == null)
                    {
                        item.ImageFinal = new Image();
                        item.ImageFinal.Source = GetSource(item);
                    }

                    newItems.Add(item);
                }
                Items.AddRange(newItems);
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
        async void OnItemSelected(Categ item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ItemsPage)}?{nameof(ItemsViewModel.Canal)}={canal}&{nameof(ItemsViewModel.RefId)}={refId}&{nameof(ItemsViewModel.CategId)}={item.CategoryId}");

        }
        async void AllProducts()
        {
            await Shell.Current.GoToAsync($"{nameof(ItemsPage)}?{nameof(ItemsViewModel.Canal)}={canal}&{nameof(ItemsViewModel.RefId)}={refId}&{nameof(ItemsViewModel.CategId)}=0");
        }
    }
}