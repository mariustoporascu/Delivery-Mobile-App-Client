using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(Canal), nameof(Canal))]
    [QueryProperty(nameof(RefId), nameof(RefId))]
    [QueryProperty(nameof(CategId), nameof(CategId))]
    public class ItemsViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Grouping<Categ, Item>> _items;
        public ObservableRangeCollection<Grouping<Categ, Item>> Items { get => _items; set => SetProperty(ref _items, value); }

        private ObservableRangeCollection<Grouping<SubCateg, Item>> _itemsSubCateg;
        public ObservableRangeCollection<Grouping<SubCateg, Item>> ItemsSubCateg { get => _itemsSubCateg; set => SetProperty(ref _itemsSubCateg, value); }
        public List<Item> SItems { get; set; }
        public List<Categ> SCateg { get; set; }
        public List<SubCateg> SSubCateg { get; set; }
        public List<CartItem> CItems { get; set; }
        private string searchItem = "";
        private Item _selectedItem;
        private int categId;
        private int canal;
        private int refId;
        public Command LoadItemsCommand { get; }
        public Command<Item> MinusCommand { get; }
        public Command<Item> PlusCommand { get; }
        public Command SearchCommand { get; }
        public Command<Item> ItemTapped { get; }
        public string SearchItem
        {
            get => searchItem;
            set => SetProperty(ref searchItem, value);
        }
        public int CategId
        {
            get
            {
                return categId;
            }
            set
            {
                categId = value;
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
        public ItemsViewModel()
        {
            Title = "Produse";
            Items = new ObservableRangeCollection<Grouping<Categ, Item>>();
            ItemsSubCateg = new ObservableRangeCollection<Grouping<SubCateg, Item>>();
            CItems = new List<CartItem>();
            SItems = new List<Item>();
            SCateg = new List<Categ>();
            SSubCateg = new List<SubCateg>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Item>((item) => OnItemSelected(item));
            MinusCommand = new Command<Item>(OnMinus);
            PlusCommand = new Command<Item>(OnPlus);
            SearchCommand = new Command(Searching);
        }

        void ExecuteLoadItemsCommand()
        {

            try
            {
                SearchItem = "";
                Items.Clear();
                ItemsSubCateg.Clear();
                var newList = new ObservableRangeCollection<Grouping<Categ, Item>>();
                var newListSub = new ObservableRangeCollection<Grouping<SubCateg, Item>>();
                if (SItems.Count == 0)
                {
                    var items = DataStore.GetItems(canal, refId, categId);
                    foreach (var item in items)
                    {
                        item.Cantitate = 0;
                        SItems.Add(item);
                    }
                }
                if (SCateg.Count == 0)
                {
                    var items = DataStore.GetCategories(canal, refId);
                    foreach (var item in items)
                    {
                        SCateg.Add(item);
                    }
                }
                if (SSubCateg.Count == 0)
                {
                    var items = DataStore.GetSubCategories();
                    foreach (var item in items)
                    {
                        SSubCateg.Add(item);
                    }
                }
                foreach (var categ in SCateg)
                {
                    if (canal == 1)
                    {
                        if (SSubCateg.FindAll(subCategs => subCategs.CategoryRefId == categ.CategoryId).Count > 0)
                            foreach (var subCateg in SSubCateg)
                            {
                                if (subCateg.CategoryRefId == categ.CategoryId)
                                {
                                    if (SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                                    {
                                        newListSub.Add(new Grouping<SubCateg, Item>(subCateg, SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                                    }
                                }
                            }

                    }
                    else
                    {
                        if (SItems.FindAll(item => item.CategoryRefId == categ.CategoryId).Count > 0)
                            newList.Add(new Grouping<Categ, Item>(categ, SItems.FindAll(item => item.CategoryRefId == categ.CategoryId)));
                    }

                }
                if (newList.Count > 0)
                {
                    Items.AddRange(newList);
                }
                else
                {
                    ItemsSubCateg.AddRange(newListSub);
                }
                CItems.Clear();
                var citems = DataStore.GetCartItems();
                foreach (var citem in citems)
                {
                    var itemCHK = SItems.Find(item => item.ProductId == citem.ProductId);
                    if (itemCHK != null)
                        itemCHK.Cantitate = citem.Cantitate;
                    CItems.Add(citem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        void Searching()
        {
            if (string.IsNullOrWhiteSpace(SearchItem))
                return;
            try
            {
                Items.Clear();
                ItemsSubCateg.Clear();
                var newList = new ObservableCollection<Grouping<Categ, Item>>();
                var newListSub = new ObservableCollection<Grouping<SubCateg, Item>>();
                var items = SItems.FindAll(item => item.Name.ToLower().Contains(searchItem.ToLower())
                        || item.Description.ToLower().Contains(searchItem.ToLower()));
                foreach (var categ in SCateg)
                    if (canal == 1)
                    {
                        if (SSubCateg.FindAll(subCategs => subCategs.CategoryRefId == categ.CategoryId).Count > 0)
                            foreach (var subCateg in SSubCateg)
                            {
                                if (subCateg.CategoryRefId == categ.CategoryId)
                                {
                                    if (items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                                    {
                                        newListSub.Add(new Grouping<SubCateg, Item>(subCateg, items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                                    }
                                }
                            }
                    }
                    else
                    {
                        if (items.FindAll(item => item.CategoryRefId == categ.CategoryId).Count > 0)
                            newList.Add(new Grouping<Categ, Item>(categ, items.FindAll(item => item.CategoryRefId == categ.CategoryId)));
                    }
                if (newList.Count > 0)
                {
                    Items.AddRange(newList);
                }
                else
                {
                    ItemsSubCateg.AddRange(newListSub);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        void OnMinus(Item itemVM)
        {
            if (CItems == null)
                return;
            var item = CItems.Find(citem => citem.ProductId == itemVM.ProductId);
            if (itemVM == null || item == null)
                return;
            item.Cantitate--;
            itemVM.Cantitate--;
            if (item.Cantitate == 0)
            {
                CItems.Remove(item);
                DataStore.DeleteFromCart(item);
            }
            else
                DataStore.SaveCart(item);

        }

        void OnPlus(Item itemVM)
        {
            if (CItems == null)
                return;
            if (itemVM == null)
                return;
            var item = CItems.Find(citem => citem.ProductId == itemVM.ProductId);

            if (item == null)
            {
                item = new CartItem
                {
                    ProductId = itemVM.ProductId,
                    Description = itemVM.Description,
                    Gramaj = itemVM.Gramaj,
                    Name = itemVM.Name,
                    Price = itemVM.Price,
                    Cantitate = itemVM.Cantitate,
                    Canal = Canal,
                    ShopId = Canal == 1 ? itemVM.SuperMarketRefId : itemVM.RestaurantRefId
                };
                CItems.Add(item);
            }
            item.Cantitate++;
            itemVM.Cantitate++;
            DataStore.SaveCart(item);

        }
        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.ProductId}");
        }
    }
}