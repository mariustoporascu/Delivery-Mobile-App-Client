using LivroApp.Models.ShopModels;
using LivroApp.ViewModels;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ProductsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ProductsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if (Device.RuntimePlatform == Device.iOS)
            //    ItemsCollView.ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
            viewModel.LoadItemsCommand.Execute(null);
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

            viewModel.SearchCommand.Execute(null);
        }
        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (viewModel.SItems.Count > 0)
        //        viewModel.Searching();
        //}
    }
}