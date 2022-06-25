using FoodDeliveryApp.Models.AuthModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class UserLocationsListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<UserLocation> UserLocations { get; set; }
        private bool _canAddMore;
        public bool CanAddMore { get => _canAddMore; set => SetProperty(ref _canAddMore, value); }
        public event EventHandler EditLocation = delegate { };
        public event EventHandler DeleteLocationFailed = delegate { };
        public Command<UserLocation> ItemTapped { get; }
        public Command<UserLocation> DeleteCommand { get; }
        public int LocationId { get; set; }
        public UserLocationsListViewModel()
        {
            UserLocations = new ObservableRangeCollection<UserLocation>();
            if (App.UserInfo.Locations != null && App.UserInfo.Locations.Count > 0)
                UserLocations.AddRange(App.UserInfo.Locations);

            if (UserLocations.Count > 2)
                CanAddMore = false;
            else
                CanAddMore = true;
            ItemTapped = new Command<UserLocation>((item) => EditItem(item));
            DeleteCommand = new Command<UserLocation>(async (item) => await Delete(item));
            IsBusy = false;

        }

        public void RefreshLocations()
        {
            UserLocations.Clear();
            if (App.UserInfo.Locations != null && App.UserInfo.Locations.Count > 0)
                UserLocations.AddRange(App.UserInfo.Locations);

            if (UserLocations.Count > 2)
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
                {
                    DeleteLocationFailed?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception)
            {
                DeleteLocationFailed?.Invoke(this, new EventArgs());
            }

        }
    }
}
