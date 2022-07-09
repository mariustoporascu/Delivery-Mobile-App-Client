using LivroApp.Models.AuthModels;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.UserVModels
{
    public class UserLocationsListViewModel : BaseViewModel<UserLocation>
    {
        private bool _canAddMore;
        public bool CanAddMore { get => _canAddMore; set => SetProperty(ref _canAddMore, value); }
        public event EventHandler EditLocation = delegate { };
        public Command<UserLocation> DeleteCommand { get; }
        public int LocationId { get; set; }
        public UserLocationsListViewModel()
        {
            Items = new ObservableRangeCollection<UserLocation>();
            ItemTapped = new Command<UserLocation>((item) => EditItem(item));
            DeleteCommand = new Command<UserLocation>(async (item) => await Delete(item));
            RefreshLocations();
        }

        public void RefreshLocations()
        {
            Items.Clear();
            if (App.UserInfo.Locations != null && App.UserInfo.Locations.Count > 0)
                Items.AddRange(App.UserInfo.Locations);

            if (Items.Count > 2)
                CanAddMore = false;
            else
                CanAddMore = true;
        }
        void EditItem(UserLocation item)
        {
            if (item == null)
                return;
            LocationId = item.LocationId;
            EditLocation?.Invoke(this, new EventArgs());
        }
        async Task Delete(UserLocation item)
        {
            if (item == null)
                return;
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    LocationDeleteId = item.LocationId,
                    Email = App.UserInfo.Email,
                    UserIdentification = App.UserInfo.UserIdentification,
                    Password = App.UserInfo.Password,
                }, Constants.AuthOperations.DeleteLocation);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Location deleted."))
                {

                    var location = App.UserInfo.Locations.Find(loc => loc.LocationId == item.LocationId);
                    App.UserInfo.Locations.Remove(location);

                    RefreshLocations();
                }
                else
                    CallFailedEvent();
            }
            catch (Exception) { CallFailedEvent(); }
            finally { IsBusy = false; }

        }
    }
}
