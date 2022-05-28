using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IAuthController
    {
        Task<string> Execute(UserModel userModel, AuthOperations operation);
    }
}
