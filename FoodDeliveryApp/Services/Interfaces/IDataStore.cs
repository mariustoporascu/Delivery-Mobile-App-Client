using FoodDeliveryApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IDataStore
    {
        Task Init();
        void SaveCart(CartItem item);
        void DeleteFromCart(CartItem item);
        CartItem GetCartItem(int id);
        void CleanCart();
        List<CartItem> GetCartItems();
        Item GetItem(int id);
        ServerOrder GetOrder(int id);
        List<Item> GetItems(int refId, int? categId);
        //IEnumerable<Item> SearchItems(int canal, int refId, int? categId, string itemName = "");
        IEnumerable<Categ> GetCategories(int refId);
        IEnumerable<SubCateg> GetSubCategories(int? categId);
        IEnumerable<Companie> GetCompanii(int tipCompanie);
        IEnumerable<AvailableCity> GetAvailableCities();
        Companie GetCompanie(int companieId);
        IEnumerable<UnitatiMasura> GetUnitatiMasura();
        IEnumerable<TipCompanie> GetTipCompanii();
        Task<List<ServerOrder>> GetServerOrders(string email);
    }
}
