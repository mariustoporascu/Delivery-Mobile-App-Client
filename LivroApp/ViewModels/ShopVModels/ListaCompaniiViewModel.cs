using LivroApp.Models.ShopModels;
using LivroApp.Views;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace LivroApp.ViewModels.ShopVModels
{
    [QueryProperty(nameof(RefId), nameof(RefId))]
    public class ListaCompaniiViewModel : BaseViewModel<Companie>
    {
        public ListaCompaniiViewModel()
        {
            Title = DataStore.GetTipCompanii().First(tip => tip.TipCompanieId == RefId).Name;
            Items = new ObservableRangeCollection<Companie>();
            RefreshServerData = new Command(async () => await RefreshAppData());
            ItemTapped = new Command<Companie>((item) => OnItemSelected(item));
        }

        void ExecuteLoadItems()
        {
            Items.Clear();
            var items = DataStore.GetCompanii(RefId).Where(comp => comp.VisibleInApp == true);
            Items.AddRange(items);
        }
        public async Task RefreshAppData()
        {

            IsBusy = true;
            try
            {
                await ReloadServerData();
                var items = DataStore.GetCompanii(RefId).ToList();
                if (items.Count != Items.Count)
                    ExecuteLoadItems();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;

        }
        async void OnItemSelected(Companie item)
        {
            if (item == null)
                return;
            if (!item.IsActive)
            {
                CallFailedEvent();
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(RefId)}={item.CompanieId}");
        }
    }
}