using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.UserVModels
{
    public class UserDetailsViewModel : BaseViewModel<BaseModel>
    {
        private string _fullName = string.Empty;
        private string _phoneNumber = string.Empty;

        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public Command SaveProfile { get; }
        public UserDetailsViewModel()
        {
            SaveProfile = new Command(async () => await OnSaveProfile());
        }
        async Task OnSaveProfile()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = App.UserInfo.Email,
                    Password = App.UserInfo.Password,
                    UserIdentification = App.UserInfo.UserIdentification,
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    CompleteProfile = true,
                }, AuthOperations.Profile);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Profile updated."))
                {
                    App.UserInfo.FullName = FullName;
                    App.UserInfo.PhoneNumber = PhoneNumber;
                    App.UserInfo.CompleteProfile = true;
                    CallSuccessEvent();
                }
                else
                    CallFailedEvent();
            }
            catch (Exception) { CallFailedEvent(); }
            finally { IsBusy = false; }
        }
    }
}
