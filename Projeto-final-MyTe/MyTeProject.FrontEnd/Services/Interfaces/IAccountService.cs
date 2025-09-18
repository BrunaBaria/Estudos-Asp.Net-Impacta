using MyTeProject.FrontEnd.Models.UserModels;

namespace MyTeProject.FrontEnd.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> Login(LoginModel login);

        public Task<UserModel> Get();
    }
}
