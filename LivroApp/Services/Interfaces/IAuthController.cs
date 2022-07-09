using LivroApp.Constants;
using LivroApp.Models.AuthModels;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public interface IAuthController
    {
        Task<string> Execute(UserModel userModel, AuthOperations operation);
        Task<bool> FbLoginEnabled();

    }
}
