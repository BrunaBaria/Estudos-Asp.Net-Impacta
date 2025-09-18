using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class LocationService : CRUDService<LocationModel>, ILocationService
    {

        public LocationService(HttpClient httpClient) : base(httpClient, "location")
        {
        }

        public override async Task<List<LocationModel>> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<List<LocationModel>>($"/v1/{_path}/GetWithDependecies");

            return apiResponse;
        }

        public override async Task<LocationModel> Get(int id)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<LocationModel>($"/v1/{_path}/GetWithDependecies/{id}");

            return apiResponse;
        }


    }
}
