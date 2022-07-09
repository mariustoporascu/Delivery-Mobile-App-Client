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
    public class HomeViewModel : BaseViewModel<TipCompanie>
    {
        private bool firstLaunch = true;
        public HomeViewModel()
        {
            Items = new ObservableRangeCollection<TipCompanie>();
            ItemTapped = new Command<TipCompanie>((item) => OnItemSelected(item));
            LoadAllItems = new Command(ExecuteLoadItemsCommand);
            RefreshServerData = new Command(async () => await RefreshAppData());
        }
        void ExecuteLoadItemsCommand()
        {
            try
            {
                Items.Clear();
                var items = DataStore.GetTipCompanii();
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
            if (!firstLaunch)
            {
                try
                {
                    await ReloadServerData();
                    var items = DataStore.GetTipCompanii().ToList();
                    if (items.Count != Items.Count)
                        ExecuteLoadItemsCommand();
                }
                catch (Exception) { }
            }
            else
            {
                ExecuteLoadItemsCommand();
                firstLaunch = false;
            }
            IsBusy = false;
        }
        async void OnItemSelected(TipCompanie item)
        {
            if (item == null)
                return;
            if (!item.IsOpen)
            {
                CallFailedEvent();
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(ListaCompaniiPage)}?{nameof(RefId)}={item.TipCompanieId}");
        }
    }
}
