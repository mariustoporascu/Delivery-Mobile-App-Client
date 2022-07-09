using LivroApp.Models;
using LivroApp.Models.ShopModels;
using LivroApp.Services;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class ATCPopUpVM : BaseViewModel<object>
    {
        public CartItem Item { get; set; }
        public decimal RefPrice { get; set; }
        public ATCPopUpVM(CartItem item)
        {
            Item = item;
            RefPrice = DependencyService.Get<IDataStore>().GetItem(Item.ProductId).Price;
        }
    }
}
