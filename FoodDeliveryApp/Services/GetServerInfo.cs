using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private HttpClient client;
        public List<CartItem> cartItems;
        public List<Companie> companii;
        public List<ServerOrder> serverOrders;
        public List<TipCompanie> tipCompanii;
        public List<UnitatiMasura> unitati;
        public List<AvailableCity> cities;
        public List<string> paymentMethods;

        public GetServerInfo()
        {
            client = new HttpClient();
        }

        public async Task loadAppInfo()
        {
            loadCartPrefs();
            try
            {
                await loadServerCateg();
                await loadServerSubCateg();
                await loadServerProducts();
                await loadServerCompanii();
                await loadServerTipCompanii();
                await loadServerMeasuringUnits();
                await loadServerAvailableCity();
                await getPaymentMethods();
            }
            catch (Exception) { }

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
        async Task getPaymentMethods()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getpaymentmethods");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                paymentMethods = JsonConvert.DeserializeObject<List<string>>(content, settings);
            }
        }
        async Task loadServerProducts()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallproducts");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                items = JsonConvert.DeserializeObject<List<Item>>(content, settings);
            }
        }
        async Task loadServerAvailableCity()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallcities");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                cities = JsonConvert.DeserializeObject<List<AvailableCity>>(content, settings);
            }
        }

        async Task loadServerCateg()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallcategories");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
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
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                subCateg = JsonConvert.DeserializeObject<List<SubCateg>>(content, settings);
            }
        }
        async Task loadServerCompanii()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallcompanii");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                companii = JsonConvert.DeserializeObject<List<Companie>>(content, settings);
            }
        }
        async Task loadServerTipCompanii()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getalltipcompanii");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                tipCompanii = JsonConvert.DeserializeObject<List<TipCompanie>>(content, settings);
            }
        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = client.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = client.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    client.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    client.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    client.DefaultRequestHeaders.Remove("authkey");
                    client.DefaultRequestHeaders.Remove("authid");
                    client.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    client.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public async Task<List<ServerOrder>> loadServerOrders(string email)
        {
            TryAddHeaders();
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getallorders/{email}");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
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
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
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