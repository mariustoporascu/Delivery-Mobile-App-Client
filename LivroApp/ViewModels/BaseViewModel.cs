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
    public class BaseViewModel<TT> : INotifyPropertyChanged where TT : class
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
        private int? _refId;
        public int? RefId
        {
            get => _refId;
            set => SetProperty(ref _refId, value);
        }
        private ObservableRangeCollection<TT> _items;
        public ObservableRangeCollection<TT> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public Command<TT> ItemTapped { get; set; }
        public Command SearchItem { get; set; }
        public Command LoadAllItems { get; set; }
        public Command AllItemsTapped { get; set; }
        public Command RefreshServerData { get; set; }
        public bool LoggedIn { get => App.IsLoggedIn; }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public void CallSuccessEvent() { SuccessDelegate?.Invoke(this, new EventArgs()); }
        public void CallFailedEvent() { FailedDelegate?.Invoke(this, new EventArgs()); }
        public async Task ReloadServerData()
        {
            await DataStore.Init();
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
