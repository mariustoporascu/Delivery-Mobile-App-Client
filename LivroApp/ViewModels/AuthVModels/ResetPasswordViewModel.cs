using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.AuthVModels
{
    public class ResetPasswordViewModel : BaseViewModel<BaseModel>
    {
        private string _confirmPassword = string.Empty;
        private string _token = string.Empty;
        private string _newpassword = string.Empty;
        private string _userName = string.Empty;
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Token { get => _token; set => SetProperty(ref _token, value); }
        public string NewPassword { get => _newpassword; set => SetProperty(ref _newpassword, value); }
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public event EventHandler CoolDown = delegate { };

        public Command ResetPassword { get; }
        public ResetPasswordViewModel()
        {
            ResetPassword = new Command(async () => await ChangingPass());
        }
        private async Task ChangingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    ResetTokenPass = Token,
                    Email = UserName,
                    NewPassword = NewPassword,
                }, AuthOperations.ResetPassword);

                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password changed"))
                    SuccessDelegate?.Invoke(this, new EventArgs());
                else if (!string.IsNullOrWhiteSpace(result) && result.Contains("Tried too many times"))
                    CoolDown?.Invoke(this, new EventArgs());
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }
    }
}
