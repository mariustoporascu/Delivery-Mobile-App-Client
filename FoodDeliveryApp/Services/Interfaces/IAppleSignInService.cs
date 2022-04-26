using FoodDeliveryApp.Models.AuthModels;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }

        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);

        Task<AppleAccount> SignInAsync();
    }

}