using LivroApp.ViewModels.UserVModels;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class AskHelpPopUp : Popup
    {
        AskHelpPopUpVM viewModel;
        public AskHelpPopUp()
        {
            InitializeComponent();
            BindingContext = viewModel = new AskHelpPopUpVM();
        }
        void OnDismissButtonClicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        async void SendClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.Help.Email) && !string.IsNullOrWhiteSpace(viewModel.Help.Name)
                && !string.IsNullOrWhiteSpace(viewModel.Help.TelNo) && !string.IsNullOrWhiteSpace(viewModel.Help.Message))
            {
                try
                {
                    if (await viewModel.SendMsg())
                    {
                        await Shell.Current.DisplayAlert("Succes", "Mesajul tau a fost trimis.", "OK");
                        Dismiss(null);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Eroare", "Mesajul nu a fost trimis, reincercati.", "OK");
                        return;
                    }
                }
                catch (Exception) { }
            }
            else
                await Shell.Current.DisplayAlert("Eroare", "Trebuie sa completezi toate campurile.", "OK");
        }
    }
}