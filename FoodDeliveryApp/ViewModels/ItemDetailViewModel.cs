using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Diagnostics;
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
                LoadItem(value);
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
                    Canal = Item.SuperMarketRefId != null ? 1 : 2,
                    ShopId = Item.SuperMarketRefId != null ? Item.SuperMarketRefId : Item.RestaurantRefId
                };
            }
            Item.Cantitate++;
            CItem.Cantitate++;
            DataStore.SaveCart(CItem);

        }
        public void LoadItem(int itemId)
        {
            try
            {
                Item = DataStore.GetItem(itemId);
                Title = "Detalii " + Item.Name;
                CItem = DataStore.GetCartItem(itemId);
                if (CItem != null)
                    Item.Cantitate = CItem.Cantitate;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
