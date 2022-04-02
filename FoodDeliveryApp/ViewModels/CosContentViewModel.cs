using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class CosContentViewModel : BaseViewModel
    {
        private CartItem _selectedItem;
        private decimal _total;
        private ObservableCollection<CartItem> _items;
        public ObservableCollection<CartItem> Items { get => _items; set => SetProperty(ref _items, value); }

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

            ItemTapped = new Command<CartItem>(OnItemSelected);

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
            IsBusy = true;

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
            PlusCommand.ChangeCanExecute();
            DeleteCommand.ChangeCanExecute();
            MinusCommand.ChangeCanExecute();
        }
        async void OnItemSelected(CartItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.ProductId}");
        }
    }
}
