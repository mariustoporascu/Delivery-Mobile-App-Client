using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(RefId), nameof(RefId))]
    public class CategoryViewModel : BaseViewModel<Category>
    {
        public CategoryViewModel()
        {
            Items = new ObservableRangeCollection<Category>();
            LoadAllItems = new Command(ExecuteLoadItems);
            ItemTapped = new Command<Category>((item) => OnItemSelected(item));
            AllItemsTapped = new Command(OnAllProducts);
        }
        void ExecuteLoadItems()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var items = DataStore.GetCategories((int)RefId);
                Items.AddRange(items);
                if (Items.Count > 0)
                    IsAvailable = true;
                else
                    IsAvailable = false;
            }
            catch (Exception) { }
            finally { IsBusy = false; }
        }
        public async void OnItemSelected(Category item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(RefId)}={RefId}&{nameof(ProductsViewModel.CategId)}={item.CategoryId}");

        }
        async void OnAllProducts()
        {
            await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(RefId)}={RefId}&{nameof(ProductsViewModel.CategId)}={0}");
        }
    }
}