using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;
using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Models.WBSModels;
using MyTeProject.FrontEnd.Models.Filters;
using System.Reflection;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class DepartmentService : CRUDService<DepartmentModel>, IDepartmentService
    {

        public DepartmentService(HttpClient httpClient) : base(httpClient, "department")
        {
        }

        public override async Task<List<DepartmentModel>> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<List<DepartmentModel>>($"/v1/{_path}/GetWithDependecies");

            return apiResponse;
        }

        public override async Task<DepartmentModel> Get(int id)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<DepartmentModel>($"/v1/{_path}/GetWithDependecies/{id}");

            return apiResponse;
        }

        public async Task<List<DepartmentModel>> Get(DepartmentFilter filter)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync($"/v1/{_path}/GetWithFilters", filter);

            apiResponse.EnsureSuccessStatusCode();

            return await apiResponse.Content.ReadFromJsonAsync<List<DepartmentModel>>();
        }

    }
}
