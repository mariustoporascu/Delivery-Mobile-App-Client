using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.AuthVModels
{
    public class ConfirmEmailViewModel : BaseViewModel<object>
    {
        private string _userName = string.Empty;
        private string _token = string.Empty;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public string Token { get => _token; set => SetProperty(ref _token, value); }
        public Command ConfirmEmail { get; set; }
        public ConfirmEmailViewModel()
        {
            ConfirmEmail = new Command(async () => await Confirm());
        }
        public async Task Confirm()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = UserName,
                    ResetTokenPass = Token,
                }, AuthOperations.ConfirmEmail);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Email Confirmed."))
                    CallSuccessEvent();
                else
                    CallFailedEvent();
            }
            catch (Exception) { CallFailedEvent(); }
            finally { IsBusy = false; }

        }
    }
}
