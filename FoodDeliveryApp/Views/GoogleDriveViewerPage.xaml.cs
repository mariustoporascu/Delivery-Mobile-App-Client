using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class GoogleDriveViewerPage : ContentPage
    {
        HttpClient client;
        string url = string.Empty;
        public GoogleDriveViewerPage(string uri)
        {
            InitializeComponent();
            client = new HttpClient();
            url = uri;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var filePath = await GetPdf(url);
            DocField.Uri = filePath;
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        async Task<string> GetPdf(string url)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, url.Split('/').Last());

            var pdfBytes = await client.GetByteArrayAsync(url);

            try
            {
                File.WriteAllBytes(filePath, pdfBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Pdf-ul nu a putut fi incarcat, redeschideti pagina.", "OK");
            }

            return string.Empty;
        }
    }
}