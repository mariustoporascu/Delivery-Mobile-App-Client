using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.ViewModels
{
    public class ATCPopUpVM : BaseViewModel
    {
        public CartItem Item { get; set; }
        public decimal RefPrice { get; set; }
        public ATCPopUpVM(CartItem item)
        {
            Item = item;
            RefPrice = DataStore.GetItem(Item.ProductId).Price;
        }
    }
}
