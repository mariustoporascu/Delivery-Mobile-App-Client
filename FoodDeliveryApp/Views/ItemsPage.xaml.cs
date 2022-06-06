using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

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

            ItemsListView.ItemsSource = viewModel.ItemsSubCateg;
            viewModel.LoadItemsCommand.Execute(null);
            ItemsListView.ScrollTo(0, 0, position: ScrollToPosition.Start);
            viewModel.CItems = viewModel.DataStore.GetCartItems();
        }

        private async void OnAddItem(object sender, EventArgs e)
        {
            var btnDetails = (ImageButton)sender;
            var cartItem = viewModel.CItems.Find(ci => ci.ProductId == ((Item)btnDetails.CommandParameter).ProductId);
            var item = (Item)btnDetails.CommandParameter;
            if (viewModel.CheckHasAnother())
            {
                var prompt = await DisplayAlert("Confirmati",
                    "Aveti in cos produse de la alta companie. Cosul va fi curatat pentru adaugarea acestui produs", "OK", "Cancel");
                if (prompt)
                {
                    viewModel.DataStore.CleanCart();
                }
                else
                    return;
            }
            Navigation.ShowPopup(new ATCPopUp(cartItem ?? new CartItem
            {
                ProductId = item.ProductId,
                Name = item.Name,
                Gramaj = item.Gramaj,
                Cantitate = 1,
                PriceTotal = item.Price,
                CompanieRefId = viewModel.RefId
            }));
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