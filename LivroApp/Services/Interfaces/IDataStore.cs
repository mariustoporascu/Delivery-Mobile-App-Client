using LivroApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public interface IDataStore
    {
        Task Init();
        List<string> GetPaymentMtds();
        void SaveCart(CartItem item);
        void DeleteFromCart(CartItem item);
        CartItem GetCartItem(int id);
        void CleanCart();
        List<CartItem> GetCartItems();
        Product GetItem(int id);
        ServerOrder GetOrder(int id);
        List<Product> GetItems(int refId, int? categId);
        //IEnumerable<Item> SearchItems(int canal, int refId, int? categId, string itemName = "");
        IEnumerable<Category> GetCategories(int refId);
        IEnumerable<SubCategory> GetSubCategories(int? categId);
        IEnumerable<Companie> GetCompanii(int tipCompanie);
        IEnumerable<AvailableCity> GetAvailableCities();
        Companie GetCompanie(int companieId);
        IEnumerable<UnitatiMasura> GetUnitatiMasura();
        IEnumerable<TipCompanie> GetTipCompanii();
        Task<List<ServerOrder>> GetServerOrders(string email);
    }
}
