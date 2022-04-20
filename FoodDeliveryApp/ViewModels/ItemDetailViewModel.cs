using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private CartItem cItem;
        private Item item;
        private int itemId;
        public Command MinusCommand { get; }
        public Command PlusCommand { get; }

        public ItemDetailViewModel()
        {
            Title = "Detalii Produs";
            MinusCommand = new Command(OnMinus);
            PlusCommand = new Command(OnPlus);
        }
        public Item Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }
        public CartItem CItem
        {
            get => cItem;
            set => SetProperty(ref cItem, value);
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
                LoadItemId(value);
            }
        }
        void OnMinus()
        {
            if (Item.Cantitate == 0)
                return;
            Item.Cantitate--;
            CItem.Cantitate--;
            if (CItem.Cantitate == 0)
            {
                DataStore.DeleteFromCart(CItem);
            }
            else
                DataStore.SaveCart(CItem);

        }

        void OnPlus()
        {
            if (Item == null)
                return;

            if (CItem == null)
            {
                CItem = new CartItem
                {
                    ProductId = Item.ProductId,
                    Description = Item.Description,
                    Gramaj = Item.Gramaj,
                    Name = Item.Name,
                    Price = Item.Price,
                    Cantitate = Item.Cantitate,
                };
            }
            Item.Cantitate++;
            CItem.Cantitate++;
            DataStore.SaveCart(CItem);

        }
        public void LoadItemId(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
                CItem = DataStore.GetCartItem(itemId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
