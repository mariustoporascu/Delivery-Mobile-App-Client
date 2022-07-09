using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.AuthVModels
{
    public class SetPasswordViewModel : BaseViewModel<BaseModel>
    {
        private string _confirmPassword = string.Empty;
        private string _password = string.Empty;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public Command SetPassword { get; }
        public SetPasswordViewModel()
        {
            SetPassword = new Command(async () => await SettingPass());
        }
        private async Task SettingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = App.UserInfo.Email,
                    Password = Password,
                    UserIdentification = App.UserInfo.UserIdentification,
                }, AuthOperations.SetPassword);

                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password set."))
                {
                    App.UserInfo.HasSetPassword = true;
                    SuccessDelegate?.Invoke(this, new EventArgs());
                }
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }
    }
}
