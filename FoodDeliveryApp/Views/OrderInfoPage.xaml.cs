using FoodDeliveryApp.ViewModels;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class OrderInfoPage : ContentPage
    {
        public OrderInfoPage()
        {
            InitializeComponent();
            BindingContext = new OrderInfoViewModel();
        }
    }
}