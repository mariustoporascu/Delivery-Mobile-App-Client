using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace LivroApp.ViewModels.AuthVModels
{
    public class RegisterViewModel : BaseViewModel<object>
    {
        private string _userName = string.Empty;
        private string _fullName = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _password = string.Empty;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public Command SignUpLivro { get; set; }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };


        public RegisterViewModel()
        {
            SignUpLivro = new Command(async () => await SignUpWithWeb());
        }

        private async Task SignUpWithWeb()
        {
            IsBusy = true;
            try
            {
                var serverResp = await AuthController.Execute(new UserModel
                {
                    FullName = FullName,
                    Email = UserName,
                    Password = Password,
                    FireBaseToken = App.FirebaseUserToken

                }, AuthOperations.Create);

                if (!string.IsNullOrEmpty(serverResp) && serverResp.Contains("Account created, you can now login."))
                    SuccessDelegate?.Invoke(this, new EventArgs());
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }

    }
}
