using LivroApp.Models;
using LivroApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels
{
    public class BaseViewModel<T> : BaseModel where T : class
    {
        public IDataStore DataStore => DependencyService.Get<IDataStore>();
        public IAuthController AuthController => DependencyService.Get<IAuthController>();
        public IOrderServ OrderService => DependencyService.Get<IOrderServ>();

        bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        bool _isAvailable = false;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { SetProperty(ref _isAvailable, value); }
        }
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private int _refId;
        public int RefId
        {
            get => _refId;
            set => SetProperty(ref _refId, value);
        }
        private ObservableRangeCollection<T> _items;
        public ObservableRangeCollection<T> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command<T> ItemTapped { get; set; }
        public Command SearchItem { get; set; }
        public Command LoadAllItems { get; set; }
        public Command AllItemsTapped { get; set; }
        public Command RefreshServerData { get; set; }
        public bool LoggedIn { get => App.IsLoggedIn; }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public void CallSuccessEvent() { SuccessDelegate?.Invoke(this, new EventArgs()); }
        public void CallFailedEvent() { FailedDelegate?.Invoke(this, new EventArgs()); }
        public async Task ReloadServerData() { await DataStore.Init(); }
    }
}
