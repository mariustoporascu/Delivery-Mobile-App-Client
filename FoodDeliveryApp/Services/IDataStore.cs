using FoodDeliveryApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IDataStore
    {
        Task Init();
        void SaveCart(CartItem item);
        void DeleteFromCart(CartItem item);
        List<CartItem> GetCartItems();
        Item GetItem(int id);
        List<Item> GetItems(int canal, int refId, int? categId);
        //IEnumerable<Item> SearchItems(int canal, int refId, int? categId, string itemName = "");
        IEnumerable<Categ> GetCategories(int canal, int refId);
        IEnumerable<SubCateg> GetSubCategories();
        IEnumerable<Companie> GetRestaurante();
        IEnumerable<Companie> GetSuperMarkets();
        IEnumerable<UnitatiMasura> GetUnitatiMasura();

    }
}
