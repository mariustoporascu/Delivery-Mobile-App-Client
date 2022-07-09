using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.AuthVModels
{
    public class GenerateTokenViewModel : BaseViewModel<BaseModel>
    {
        private string _userName = string.Empty;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public Command GenerateToken { get; set; }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public event EventHandler HasCode = delegate { };

        public GenerateTokenViewModel()
        {
            GenerateToken = new Command(async () => await Generate());
        }
        public async Task Generate()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = UserName,
                }, AuthOperations.GenerateToken);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Token sent."))
                    SuccessDelegate?.Invoke(this, new EventArgs());
                else if (!string.IsNullOrWhiteSpace(result) && result.Contains("Already generated"))
                    HasCode?.Invoke(this, new EventArgs());
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }
    }
}
