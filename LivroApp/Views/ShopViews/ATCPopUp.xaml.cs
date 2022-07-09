using LivroApp.Models.ShopModels;
using LivroApp.ViewModels.ShopVModels;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ATCPopUp : Popup
    {
        ATCPopUpVM viewModel;
        public ATCPopUp(CartItem item)
        {
            InitializeComponent();
            BindingContext = viewModel = new ATCPopUpVM(item);
        }

        void OnDismissButtonClicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
        void MinusClicked(object sender, EventArgs e)
        {
            if (viewModel.Item.Cantitate == 1)
                return;
            viewModel.Item.Cantitate--;
            viewModel.Item.PriceTotal = viewModel.Item.Cantitate * viewModel.RefPrice;
        }
        void PlusClicked(object sender, EventArgs e)
        {
            viewModel.Item.Cantitate++;
            viewModel.Item.PriceTotal = viewModel.Item.Cantitate * viewModel.RefPrice;
        }
        async void AddClicked(object sender, EventArgs e)
        {
            viewModel.DataStore.SaveCart(viewModel.Item);
            Dismiss(null);
            await Shell.Current.DisplayToastAsync("Produsul a fost adaugat/modificat in cos.", 1500);

        }
    }
}