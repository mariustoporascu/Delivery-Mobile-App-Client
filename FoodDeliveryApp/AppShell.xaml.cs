using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public IDataStore DataStore => DependencyService.Get<IDataStore>();

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(ListaRestaurantePage), typeof(ListaRestaurantePage));
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Task.Run(async () => await DataStore.Init()).Wait();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
