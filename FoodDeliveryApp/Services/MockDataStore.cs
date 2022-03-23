﻿using Android.Widget;
using FoodDeliveryApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodDeliveryApp.Services
{
    public class MockDataStore : IDataStore
    {
        readonly GetServerInfo _serverInfo;

        public MockDataStore()
        {
            _serverInfo = new GetServerInfo();
        }
        public async Task Init()
        {
            await _serverInfo.loadAppInfo();
        }

        public Item GetItem(int id)
        {
            return _serverInfo.items.FirstOrDefault(s => s.ProductId == id);
        }

        public List<Item> GetItems(int canal, int refId, int? categId)
        {
            if (canal == 1)
            {
                if (categId > 0)
                {
                    return _serverInfo.items.FindAll(items => items.CategoryRefId == categId && items.SuperMarketRefId != null);
                }
                return _serverInfo.items.FindAll(items => items.SuperMarketRefId != null);
            }
            else
            {
                if (categId > 0)
                {
                    return _serverInfo.items.FindAll(items => items.CategoryRefId == categId && items.RestaurantRefId == refId);
                }
                return _serverInfo.items.FindAll(items => items.RestaurantRefId == refId);
            }
        }
        //public IEnumerable<Item> SearchItems(int canal, int refId, int? categId, string itemName = "")
        //{
        //    if (canal == 1)
        //    {
        //        if (categId > 0)
        //        {
        //            return _serverInfo.items.FindAll(items => items.CategoryRefId == categId && items.SuperMarketRefId != null
        //                && (items.Name.ToLower().Contains(itemName.ToLower())
        //                || items.Description.ToLower().Contains(itemName.ToLower())));
        //        }
        //        return _serverInfo.items.FindAll(items => items.Name.ToLower().Contains(itemName.ToLower())
        //            || items.Description.ToLower().Contains(itemName.ToLower()) && items.SuperMarketRefId != null);
        //    }
        //    else
        //    {
        //        if (categId > 0)
        //        {
        //            return _serverInfo.items.FindAll(items => items.CategoryRefId == categId && items.RestaurantRefId == refId
        //                && (items.Name.ToLower().Contains(itemName.ToLower())
        //                || items.Description.ToLower().Contains(itemName.ToLower())));
        //        }
        //        return _serverInfo.items.FindAll(items => items.Name.ToLower().Contains(itemName.ToLower())
        //            || items.Description.ToLower().Contains(itemName.ToLower()) && items.RestaurantRefId == refId);
        //    }
        //}
        public void SaveCart(CartItem item)
        {

            if (_serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId) != null)
            {
                _serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId).Cantitate = item.Cantitate;
            }
            else
            {
                _serverInfo.cartItems.Add(item);
            }
            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }
        public void DeleteFromCart(CartItem item)
        {
            if (item != null)
                _serverInfo.cartItems.Remove(item);
            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }

        public List<CartItem> GetCartItems()
        {
            _serverInfo.loadCartPrefs();

            return _serverInfo.cartItems;
        }

        public IEnumerable<Categ> GetCategories(int canal, int refId)
        {
            if (canal == 1)
            {
                return _serverInfo.categ.FindAll(categ => categ.SuperMarketRefId != null);
            }
            else
            {
                return _serverInfo.categ.FindAll(categ => categ.RestaurantRefId == refId);
            }
        }


        public IEnumerable<Companie> GetRestaurante()
        {
            return _serverInfo.restaurante;
        }

        public IEnumerable<Companie> GetSuperMarkets()
        {
            return _serverInfo.superMarkets;
        }

        public IEnumerable<UnitatiMasura> GetUnitatiMasura()
        {
            return _serverInfo.unitati;
        }

        public IEnumerable<SubCateg> GetSubCategories()
        {
            return _serverInfo.subCateg;
        }
    }
}