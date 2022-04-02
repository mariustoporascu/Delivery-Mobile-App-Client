using System;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.AuthModels;

namespace FoodDeliveryApp.Services
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }

        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);

        Task<AppleAccount> SignInAsync();
    }

}