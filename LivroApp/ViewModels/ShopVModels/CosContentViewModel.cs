using LivroApp.Constants;
using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class CosContentViewModel : BaseViewModel<CartItem>
    {
        private decimal _total;
        private List<Product> _SItems;
        HttpClient _client;
        private bool _canPlaceOrder = false;
        public bool CanPlaceOrder
        {
            get => _canPlaceOrder;
            set => SetProperty(ref _canPlaceOrder, value);
        }
        public Command MinusCommand { get; }
        public Command PlusCommand { get; }
        public Command DeleteCommand { get; }
        public CosContentViewModel()
        {
            _client = new HttpClient();
            Items = new ObservableRangeCollection<CartItem>();
            _SItems = new List<Product>();
            LoadAllItems = new Command(ExecuteLoadItemsCommand);
            MinusCommand = new Command<CartItem>(OnMinus);
            PlusCommand = new Command<CartItem>(OnPlus);
            DeleteCommand = new Command<CartItem>(OnDelete);
        }
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                Total = 0;
                var items = DataStore.GetCartItems();
                if (_SItems.Count == 0)
                    _SItems.AddRange(DataStore.GetItems(0, 0));
                foreach (var item in items)
                {
                    Items.Add(item);
                    Total += item.PriceTotal;
                }
                if (items.Count > 0)
                    IsAvailable = true;
                else
                    IsAvailable = false;
            }
            catch (Exception) { }
            finally { IsBusy = false; }
        }
        void OnDelete(CartItem item)
        {
            Items.Remove(item);
            DataStore.DeleteFromCart(item);
            RefreshCanExecutes();

        }
        void OnMinus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate--;
            item.PriceTotal = item.Cantitate * _SItems.Find(prod => prod.ProductId == item.ProductId).Price;
            if (item.Cantitate == 0)
            {
                DataStore.DeleteFromCart(item);
                Items.Remove(item);
            }
            else
                DataStore.SaveCart(item);
            GetTime();
            RefreshCanExecutes();
        }
        void OnPlus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate++;
            item.PriceTotal = item.Cantitate * _SItems.Find(prod => prod.ProductId == item.ProductId).Price;

            DataStore.SaveCart(item);
            RefreshCanExecutes();
        }
        void RefreshCanExecutes()
        {
            Total = 0;
            foreach (var item in Items)
            {
                Total = Total + item.PriceTotal;
            }
            if (Items.Count > 0)
                IsAvailable = true;
            else
                IsAvailable = false;

        }
        public async void GetTime()
        {
            try
            {
                Uri uri = new Uri(ServerConstants.TimeUrl);
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var timeObject = JsonConvert.DeserializeObject<WorldTime>(content, settings);
                    CanPlaceOrder = true;
                    var tipCompanii = DataStore.GetTipCompanii().ToList();
                    var companii = DataStore.GetCompanii(0).ToList();
                    foreach (var item in Items)
                        if (CanPlaceOrder)
                        {
                            var companie = companii.Find(comp => comp.CompanieId == item.CompanieRefId);
                            var tipCompanie = tipCompanii.Find(tip => tip.TipCompanieId == companie.TipCompanieRefId);
                            if (tipCompanie.StartHour <= tipCompanie.EndHour)
                            {
                                // start and stop times are in the same day
                                if (!(new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) >= new TimeSpan(tipCompanie.StartHour, 30, 0)
                                        && new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) <= new TimeSpan(tipCompanie.EndHour, 30, 0)))
                                {
                                    CanPlaceOrder = false;
                                }
                            }
                            else
                            {
                                // start and stop times are in different days
                                if (!(new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) >= new TimeSpan(tipCompanie.StartHour, 30, 0)
                                    || new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) <= new TimeSpan(tipCompanie.EndHour, 30, 0)))
                                {
                                    CanPlaceOrder = false;
                                }
                            }

                        }
                }
                else { CanPlaceOrder = false; }
            }
            catch (Exception) { CanPlaceOrder = false; }

        }
        partial class WorldTime
        {
            public DateTime DateTime { get; set; }
        }
    }
}
