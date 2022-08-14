using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(RefId), nameof(RefId))]
    [QueryProperty(nameof(CategId), nameof(CategId))]
    public class ProductsViewModel : BaseViewModel<Product>
    {
        private ObservableRangeCollection<Grouping<SubCategory, Product>> _itemsSubCateg;
        public ObservableRangeCollection<Grouping<SubCategory, Product>> ItemsSubCateg { get => _itemsSubCateg; set => SetProperty(ref _itemsSubCateg, value); }
        private string _searchItems = "";
        public string SearchItems { get => _searchItems; set => SetProperty(ref _searchItems, value); }
        public List<Product> SItems { get; set; }
        public List<SubCategory> SSubCateg { get; set; }
        public List<Category> SCateg { get; set; }
        public List<CartItem> CItems { get; set; }
        private int _categId;
        public int CategId { get => _categId; set => _categId = value; }
        public ProductsViewModel()
        {
            ItemsSubCateg = new ObservableRangeCollection<Grouping<SubCategory, Product>>();
            CItems = new List<CartItem>();
            SItems = new List<Product>();
            SCateg = new List<Category>();
            SSubCateg = new List<SubCategory>();
            LoadAllItems = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Product>((item) => OnItemSelected(item));
            SearchItem = new Command(Searching);
        }

        void ExecuteLoadItemsCommand()
        {

            try
            {
                SearchItems = "";
                ItemsSubCateg.Clear();
                var newListSub = new ObservableRangeCollection<Grouping<SubCategory, Product>>();
                if (SItems.Count == 0)
                {
                    var items = DataStore.GetItems(RefId, CategId);
                    foreach (var item in items)
                    {
                        SItems.Add(item);
                    }
                }
                if (SCateg.Count == 0)
                {
                    var items = DataStore.GetCategories(RefId);
                    foreach (var item in items)
                    {
                        SCateg.Add(item);
                    }
                }
                if (SSubCateg.Count == 0)
                {
                    var items = DataStore.GetSubCategories(CategId);
                    foreach (var item in items)
                    {
                        SSubCateg.Add(item);
                    }
                }
                if (CategId > 0)
                    foreach (var subCateg in SSubCateg)
                    {
                        if (subCateg.CategoryRefId == CategId)
                        {
                            if (SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                            {
                                newListSub.Add(new Grouping<SubCategory, Product>(subCateg, SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                            }
                        }
                    }
                else
                    foreach (var categ in SCateg)
                        foreach (var subCateg in SSubCateg)
                        {
                            if (subCateg.CategoryRefId == categ.CategoryId)
                            {
                                if (SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                                {
                                    newListSub.Add(new Grouping<SubCategory, Product>(subCateg, SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                                }
                            }
                        }

                ItemsSubCateg.AddRange(newListSub);
                CItems = DataStore.GetCartItems();

            }
            catch (Exception) { }
        }
        void Searching()
        {

            try
            {
                ItemsSubCateg.Clear();
                var newListSub = new ObservableCollection<Grouping<SubCategory, Product>>();
                var items = SItems.FindAll(item => item.Name.ToLower().Contains(SearchItems.ToLower())
                        || item.Description.ToLower().Contains(SearchItems.ToLower()));
                if (CategId > 0)
                    foreach (var subCateg in SSubCateg)
                    {
                        if (subCateg.CategoryRefId == CategId)
                        {
                            if (items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                            {
                                newListSub.Add(new Grouping<SubCategory, Product>(subCateg, items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                            }
                        }
                    }
                else
                    foreach (var categ in SCateg)
                        foreach (var subCateg in SSubCateg)
                        {
                            if (subCateg.CategoryRefId == categ.CategoryId)
                            {
                                if (items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                                {
                                    newListSub.Add(new Grouping<SubCategory, Product>(subCateg, items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                                }
                            }
                        }


                ItemsSubCateg.AddRange(newListSub);
            }
            catch (Exception) { }
        }

        public bool CheckHasAnother()
        {
            var hasAnotherCompany = CItems.Find(ci => ci.CompanieRefId != RefId);
            if (hasAnotherCompany != null)
                return true;
            return false;
        }

        async void OnItemSelected(Product item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?{nameof(ProductDetailViewModel.ItemId)}={item.ProductId}");
        }
    }
}