using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodDeliveryApp.Services
{
    public class GetServerInfo
    {
        public List<Item> items;
        public List<Categ> categ;
        public List<SubCateg> subCateg;
        readonly HttpClient client;
        public List<CartItem> cartItems;
        public List<Companie> restaurante;
        public List<Companie> superMarkets;
        public List<ServerOrder> serverOrders;
        public List<UnitatiMasura> unitati;

        public GetServerInfo()
        {
            client = new HttpClient();
        }

        public async Task loadAppInfo()
        {
            await loadServerCateg().ConfigureAwait(false);
            await loadServerSubCateg().ConfigureAwait(false);
            await loadServerProducts().ConfigureAwait(false);
            await loadServerRestaurante().ConfigureAwait(false);
            await loadServerSuperMarkets().ConfigureAwait(false);
            await loadServerMeasuringUnits().ConfigureAwait(false);
            loadCartPrefs();
        }

        public void loadCartPrefs()
        {
            if (CartPrefs == "" || CartPrefs == null)
            {
                cartItems = new List<CartItem>();
                return;
            }
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            cartItems = JsonConvert.DeserializeObject<List<CartItem>>(CartPrefs, settings);
        }
        public void saveCartPrefs(List<CartItem> cartPrefs)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            CartPrefs = JsonConvert.SerializeObject(cartPrefs, settings);
        }
        async Task loadServerProducts()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallproducts");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                items = JsonConvert.DeserializeObject<List<Item>>(content, settings);
            }
        }
        async Task loadServerCateg()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallcategories");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                categ = JsonConvert.DeserializeObject<List<Categ>>(content, settings);
            }
        }
        async Task loadServerSubCateg()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallsubcategories");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                subCateg = JsonConvert.DeserializeObject<List<SubCateg>>(content, settings);
            }
        }
        async Task loadServerRestaurante()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallrestaurante");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                restaurante = JsonConvert.DeserializeObject<List<Companie>>(content, settings);
            }
        }
        async Task loadServerSuperMarkets()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallsupermarkets");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                superMarkets = JsonConvert.DeserializeObject<List<Companie>>(content, settings);
            }
        }
        public async Task<List<ServerOrder>> loadServerOrders(string email)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallorders/{email}");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                serverOrders = JsonConvert.DeserializeObject<List<ServerOrder>>(content, settings);
            }
            return serverOrders;
        }
        async Task loadServerMeasuringUnits()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallmeasuringunits");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                unitati = JsonConvert.DeserializeObject<List<UnitatiMasura>>(content, settings);
                foreach (var item in items)
                {
                    item.GramajInterfata = item.Gramaj + " " + unitati.Find(unitate => unitate.UnitId == item.MeasuringUnitId).Name;
                    item.PretInterfata = item.Price.ToString() + " RON";
                }
            }
        }

        string CartPrefs
        {
            get => Preferences.Get(nameof(CartPrefs), "");
            set => Preferences.Set(nameof(CartPrefs), value);
        }
    }
}