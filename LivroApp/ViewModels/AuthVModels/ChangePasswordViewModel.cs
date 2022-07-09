using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LivroApp.ViewModels.AuthVModels
{
    public class ChangePasswordViewModel : BaseViewModel<BaseModel>
    {
        private string _confirmPassword = string.Empty;
        private string _password = string.Empty;
        private string _newpassword = string.Empty;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string NewPassword { get => _newpassword; set => SetProperty(ref _newpassword, value); }

        public Command ChangePassword { get; set; }
        public ChangePasswordViewModel()
        {
            ChangePassword = new Command(async () => await ChangingPass());
        }
        private async Task ChangingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = App.UserInfo.Email,
                    Password = Password,
                    NewPassword = NewPassword,
                }, AuthOperations.ChangePassword);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password changed."))
                {
                    App.UserInfo.Password = NewPassword;
                    SecureStorage.SetAsync(App.LOGIN_PASSWORD, NewPassword).Wait();
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
