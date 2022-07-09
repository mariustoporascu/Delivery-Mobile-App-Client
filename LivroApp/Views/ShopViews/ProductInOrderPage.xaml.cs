using LivroApp.Constants;
using LivroApp.ViewModels.ShopVModels;
using System;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ProductInOrderPage : ContentPage
    {
        ProductInOrderViewModel vm;
        public ProductInOrderPage()
        {
            InitializeComponent();
            BindingContext = vm = new ProductInOrderViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!App.IsLoggedIn)
                await Shell.Current.Navigation.PopToRootAsync();
            else
            {
                if (!string.IsNullOrWhiteSpace(vm.Item.Photo))
                {
                    ItemImage.Source = new UriImageSource
                    {

                        Uri = new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{vm.Item.Photo}"),
                        CacheValidity = new TimeSpan(7, 0, 0, 0),
                        CachingEnabled = true,
                    };
                }
                else
                {
                    ItemImage.Source = new UriImageSource
                    {

                        Uri = new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png"),
                        CacheValidity = new TimeSpan(7, 0, 0, 0),
                        CachingEnabled = true,
                    };
                }
            }
        }
    }
}