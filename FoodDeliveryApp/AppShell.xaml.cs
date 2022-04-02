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
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(ListaRestaurantePage), typeof(ListaRestaurantePage));
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(PlaceOrderPage), typeof(PlaceOrderPage));
            Task.Run(async () => await DependencyService.Get<IDataStore>().Init()).Wait();
        }

    }
}
