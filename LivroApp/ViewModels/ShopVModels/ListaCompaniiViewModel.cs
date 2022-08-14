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
            Items = new ObservableRangeCollection<Companie>();
            LoadAllItems = new Command(ExecuteLoadItems);
            RefreshServerData = new Command(async () => await RefreshAppData());
            ItemTapped = new Command<Companie>((item) => OnItemSelected(item));
        }

        void ExecuteLoadItems()
        {
            try
            {
                Items.Clear();
                var items = DataStore.GetCompanii(RefId).Where(comp => comp.VisibleInApp == true);
                Items.AddRange(items);
                if (Items.Count > 0)
                    IsAvailable = true;
                else
                    IsAvailable = false;
            }
            catch (Exception) { }

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
            catch (Exception) { }
            finally { IsBusy = false; }

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