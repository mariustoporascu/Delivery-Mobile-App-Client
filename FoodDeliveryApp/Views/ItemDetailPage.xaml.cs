using FoodDeliveryApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}