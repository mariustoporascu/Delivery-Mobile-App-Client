using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models
{
    public class CartItem : BaseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gramaj { get; set; }
        public decimal Price { get; set; }

        private int _cantitate;
        public int Cantitate
        {
            get => _cantitate;
            set => SetProperty(ref _cantitate, value);
        }
    }
}