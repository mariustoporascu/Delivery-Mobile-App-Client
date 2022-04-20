using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Canal == 1)
            {
                CategListView.ItemsSource = viewModel.ItemsSubCateg;
            }
            else
            {
                CategListView.ItemsSource = viewModel.Items;
            }
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            viewModel.Searching();
        }
        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (viewModel.SItems.Count > 0)
        //        viewModel.Searching();
        //}
    }
}