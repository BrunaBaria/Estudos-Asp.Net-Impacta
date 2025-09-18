using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Models.WBSModels;
using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class WBSService : CRUDService<WBSModel>, IWBSService
    {
        public WBSService(HttpClient httpClient) : base(httpClient, "wbs")
        {
        }

        public override async Task<List<WBSModel>> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<List<WBSModel>>($"/v1/{_path}/GetWithDependecies");

            return apiResponse;
        }

        public override async Task<WBSModel> Get(int id)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<WBSModel>($"/v1/{_path}/GetWithDependecies/{id}");

            return apiResponse;
        }

    }
}
