using LivroApp.Constants;
using LivroApp.ViewModels.ShopVModels;
using System;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel vm;
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = vm = new ItemDetailViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

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