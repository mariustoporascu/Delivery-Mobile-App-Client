using FoodDeliveryApp.ViewModels;
using System;
using Xamarin.Forms;

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

            if (viewModel.Canal == 1)
            {
                ItemsListView.ItemsSource = viewModel.ItemsSubCateg;
            }
            else
            {
                ItemsListView.ItemsSource = viewModel.Items;
            }
            viewModel.LoadItemsCommand.Execute(null);
            ItemsListView.ScrollTo(0, 0, position: ScrollToPosition.Start);
            base.OnAppearing();

        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SearchItem))
                return;
            viewModel.SearchCommand.Execute(null);
        }
        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (viewModel.SItems.Count > 0)
        //        viewModel.Searching();
        //}
    }
}