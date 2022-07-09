using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LivroApp.Views
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
            try
            {
                var filePath = await GetPdf(url);
                DocField.Uri = filePath;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Eroare", "Pdf-ul nu a putut fi incarcat, redeschideti pagina.", "OK");
            }
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


            File.WriteAllBytes(filePath, pdfBytes);

            return filePath;
        }
    }
}