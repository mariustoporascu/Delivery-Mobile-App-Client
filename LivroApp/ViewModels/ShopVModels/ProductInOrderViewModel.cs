using LivroApp.Models.ShopModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ProductInOrderViewModel : BaseViewModel
    {
        private Item item;
        private int itemId;

        public ProductInOrderViewModel()
        {
        }
        public Item Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItem(value);
            }
        }
        public void LoadItem(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
                Title = "Detalii " + Item.Name;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
