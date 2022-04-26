using FoodDeliveryApp.ViewModels;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class EntryFoodAppPage : ContentPage
    {

        public EntryFoodAppPage()
        {
            InitializeComponent();
            BindingContext = new EntryFoodAppViewModel();
        }

    }
}