using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Interfaces;
using System.Reflection;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class AccountService : IAccountService
    {
        protected readonly HttpClient _httpClient;
        protected const string Uri = "http://localhost:36881";
        protected readonly string _path;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Uri);
            _path = "account";
        }
        public async Task<bool> Login(LoginModel login)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync($"/v1/{_path}/login", login);

            apiResponse.EnsureSuccessStatusCode();

            return apiResponse.IsSuccessStatusCode;

        }
        public async Task<UserModel> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<UserModel>($"/v1/{_path}");

            return apiResponse;
        }
    }
}
