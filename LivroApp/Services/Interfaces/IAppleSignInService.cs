using LivroApp.Models.AuthModels;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }

        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);

        Task<AppleAccount> SignInAsync();
    }

}