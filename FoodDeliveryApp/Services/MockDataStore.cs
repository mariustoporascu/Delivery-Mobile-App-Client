using FoodDeliveryApp.Models.ShopModels;
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

        public void SaveCart(CartItem item)
        {

            if (_serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId) != null)
            {
                _serverInfo.items.Find(sItem => sItem.ProductId == item.ProductId).Cantitate = item.Cantitate;

                _serverInfo.cartItems.Find(citem => citem.ProductId == item.ProductId).Cantitate = item.Cantitate;
            }
            else
            {
                _serverInfo.items.Find(sItem => sItem.ProductId == item.ProductId).Cantitate = 1;

                _serverInfo.cartItems.Add(item);
            }
            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }
        public void DeleteFromCart(CartItem item)
        {
            if (item != null)
            {
                _serverInfo.items.Find(sItem => sItem.ProductId == item.ProductId).Cantitate = 0;
                _serverInfo.cartItems.Remove(item);
            }

            _serverInfo.saveCartPrefs(_serverInfo.cartItems);
        }

        public void CleanCart()
        {
            _serverInfo.cartItems.Clear();
            foreach (var item in _serverInfo.items)
            {
                item.Cantitate = 0;
            }
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
        public Companie GetRestaurant(int restaurantId)
        {
            return _serverInfo.restaurante.FirstOrDefault(r => r.RestaurantId == restaurantId);
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

        public async Task<List<ServerOrder>> GetServerOrders(string email)
        {
            return await _serverInfo.loadServerOrders(email);
        }
    }
}