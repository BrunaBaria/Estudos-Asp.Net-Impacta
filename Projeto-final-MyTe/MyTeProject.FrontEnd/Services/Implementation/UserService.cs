using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class UserService : CRUDService<UserModel>, IUserService
    {
        public UserService(HttpClient httpClient) : base(httpClient, "user")
        {
        }

        public override async Task<List<UserModel>> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<List<UserModel>>($"/v1/{_path}/GetWithDependecies");
            return apiResponse;
        }

        public override async Task<UserModel> Get(int id)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<UserModel>($"/v1/{_path}/GetWithDependecies/{id}");

            return apiResponse;
        }
    }
}
