using LivroApp.Services;
using System;

using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await DependencyService.Get<IDataStore>().Init();
            }
            catch (Exception) { }
            App.Current.MainPage = new AppShell();
        }
    }
}