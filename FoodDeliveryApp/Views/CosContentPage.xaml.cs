using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CosContentPage : ContentPage
    {
        CosContentViewModel viewModel;

        public CosContentPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CosContentViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}