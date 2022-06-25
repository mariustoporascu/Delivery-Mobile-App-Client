using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public ServerOrder GetOrder(int id)
        {
            return _serverInfo.serverOrders.FirstOrDefault(s => s.OrderId == id);
        }
        public CartItem GetCartItem(int id)
        {
            return _serverInfo.cartItems.FirstOrDefault(s => s.ProductId == id);
        }

        public List<Item> GetItems(int refId, int? categId)
        {
            if (refId == 0)
                return _serverInfo.items;
            if (categId > 0)
            {
                var subCateg = _serverInfo.subCateg.FindAll(sub => sub.CategoryRefId == categId);
                var items = new List<Item>();
                foreach (var sub in subCateg)
                    items.AddRange(_serverInfo.items.FindAll(prod => prod.SubCategoryRefId == sub.SubCategoryId));
                return items;
            }
            var categs = _serverInfo.categ.FindAll(ctg => ctg.CompanieRefId == refId);
            var subCategAll = new List<SubCateg>();
            foreach (var ctg in categs)
            {
                subCategAll.AddRange(_serverInfo.subCateg.FindAll(sub => sub.CategoryRefId == ctg.CategoryId));
            }
            var itemsAll = new List<Item>();
            foreach (var sub in subCategAll)
                itemsAll.AddRange(_serverInfo.items.FindAll(prod => prod.SubCategoryRefId == sub.SubCategoryId));
            return itemsAll;

        }

        public void SaveCart(CartItem item)
        {

            if (_serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId) != null)
            {
                var itemInside = _serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId);
                itemInside.Cantitate = item.Cantitate;
                itemInside.PriceTotal = item.PriceTotal;
                itemInside.ClientComments = item.ClientComments;
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
            {
                _serverInfo.cartItems.Remove(item);
            }

            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }

        public List<string> GetPaymentMtds()
        {
            return _serverInfo.paymentMethods;
        }
        public void CleanCart()
        {
            _serverInfo.cartItems.Clear();

            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }

        public List<CartItem> GetCartItems()
        {
            _serverInfo.loadCartPrefs();
            foreach (var item in _serverInfo.cartItems)
                if (_serverInfo.items.Find(prod => prod.ProductId == item.ProductId) == null ||
                    !_serverInfo.items.Find(prod => prod.ProductId == item.ProductId).IsAvailable)
                    DeleteFromCart(item);
            return _serverInfo.cartItems;
        }

        public IEnumerable<Categ> GetCategories(int refId)
        {
            return _serverInfo.categ.FindAll(categ => categ.CompanieRefId == refId);
        }

        public IEnumerable<Companie> GetCompanii(int tipCompanie)
        {
            if (tipCompanie == 0)
                return _serverInfo.companii;
            return _serverInfo.companii.FindAll(comp => comp.TipCompanieRefId == tipCompanie);
        }
        public Companie GetCompanie(int restaurantId)
        {
            return _serverInfo.companii.FirstOrDefault(r => r.CompanieId == restaurantId);
        }

        public IEnumerable<TipCompanie> GetTipCompanii()
        {
            return _serverInfo.tipCompanii;
        }
        public IEnumerable<AvailableCity> GetAvailableCities()
        {
            return _serverInfo.cities;
        }
        public IEnumerable<UnitatiMasura> GetUnitatiMasura()
        {
            return _serverInfo.unitati;
        }

        public IEnumerable<SubCateg> GetSubCategories(int? categId)
        {
            if (categId > 0)
                return _serverInfo.subCateg.FindAll(sub => sub.CategoryRefId == categId);
            return _serverInfo.subCateg;
        }

        public async Task<List<ServerOrder>> GetServerOrders(string email)
        {
            try
            {
                return await _serverInfo.loadServerOrders(email);

            }
            catch (Exception)
            {
                return new List<ServerOrder>();
            }
        }
    }
}