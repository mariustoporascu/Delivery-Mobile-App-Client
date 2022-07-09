using LivroApp.Models.ShopModels;
using LivroApp.ViewModels.ShopVModels;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ProductsPage : ContentPage
    {
        ProductsViewModel viewModel;

        public ProductsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProductsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadAllItems.Execute(null);
            viewModel.CItems = viewModel.DataStore.GetCartItems();
        }

        private async void OnAddItem(object sender, EventArgs e)
        {
            var btnDetails = (ImageButton)sender;
            var cartItem = viewModel.CItems.Find(ci => ci.ProductId == ((Product)btnDetails.CommandParameter).ProductId);
            var item = (Product)btnDetails.CommandParameter;
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
            viewModel.SearchItem.Execute(null);
        }

    }
}