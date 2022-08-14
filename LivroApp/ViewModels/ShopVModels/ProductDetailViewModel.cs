using LivroApp.Models.ShopModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ProductDetailViewModel : BaseViewModel<Product>
    {
        private Product _item;
        private int _itemId;
        public ProductDetailViewModel()
        {
        }
        public Product Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }
        public int ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                LoadItem(value);
            }
        }
        public void LoadItem(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
            }
            catch (Exception) { Debug.WriteLine("Failed to Load Item"); }
        }
    }
}
