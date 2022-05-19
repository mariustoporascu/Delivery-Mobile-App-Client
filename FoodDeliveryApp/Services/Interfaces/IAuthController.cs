using FoodDeliveryApp.Models.AuthModels;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IAuthController
    {
        Task<string> LoginUser(UserModel userModel);
        Task<string> CreateUser(UserModel userModel);
        Task<string> UserProfile(UserModel userModel);
        Task<string> DeleteProfile(UserModel userModel);
    }
}
