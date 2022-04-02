using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string text;
        private string description;
        private string gramaj;
        private string pret;
        private Image imageFinal;
        public int Id { get; set; }

        public string Name
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Gramaj
        {
            get => gramaj;
            set => SetProperty(ref gramaj, value);
        }

        public string Pret
        {
            get => pret;
            set => SetProperty(ref pret, value);
        }
        public Image ImageFinal
        {
            get => imageFinal;
            set => SetProperty(ref imageFinal, value);
        }
        public ItemDetailViewModel()
        {
            Title = "Detalii Produs";
        }
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        private ImageSource GetSource(Item item)
        {
            if (item.Image != null)
            {
                byte[] bytes = Convert.FromBase64String(item.Image);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return ImageSource.FromFile("No_image_available.png");

        }
        public void LoadItemId(int itemId)
        {
            try
            {
                var item = DataStore.GetItem(itemId);
                ImageFinal = new Image();
                ImageFinal.Source = GetSource(item);
                Id = item.ProductId;
                Name = item.Name;
                Description = item.Description;
                Gramaj = item.GramajInterfata;
                Pret = item.PretInterfata;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
