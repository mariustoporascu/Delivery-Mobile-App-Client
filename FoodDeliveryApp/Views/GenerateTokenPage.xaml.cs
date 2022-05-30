using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class GenerateTokenPage : ContentPage
    {
        GenerateTokenViewModel viewModel;
        public GenerateTokenPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new GenerateTokenViewModel();
            viewModel.OnSignIn += OnGenerateSucces;
            viewModel.OnSignInFailed += OnGenerateFailed;
        }
        private void CheckFieldMail(object sender, TextChangedEventArgs e)
        {
            if (!UsernameEntry.IsValid)
            {
                Email.TextColor = Color.Red;
                return;
            }
            Email.TextColor = Color.Black;
        }
        private async void CheckFields(object sender, EventArgs e)
        {

            if (!UsernameEntry.IsValid)
            {
                Email.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Email invalid", "OK");
                return;
            }

            Email.TextColor = Color.Black;


            viewModel.GenerateToken.Execute(null);
        }
        async void OnGenerateSucces(object sender, EventArgs args)
        {
            try
            {
                await this.DisplayToastAsync("Codul a fost trimis catre tine.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PushModalAsync(new ResetPasswordPage());
        }
        async void OnGenerateFailed(object sender, EventArgs args)
        {
            try
            {
                await this.DisplayAlert("Eroare", "Codul nu a putut fi generat. Reincearca.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        async void AmCodClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PushModalAsync(new ResetPasswordPage());
        }
    }
}