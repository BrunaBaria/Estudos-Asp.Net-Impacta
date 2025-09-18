using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Abstract
{
    public abstract class CRUDService<TModel> : ICRUDService<TModel>
        where TModel : class
    {
        protected readonly HttpClient _httpClient;
        protected const string Uri = "http://localhost:36881";
        protected readonly string _path;

        public CRUDService(HttpClient httpClient, string path)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Uri);
            _path = path;
        }

        public virtual async Task<List<TModel>> Get()
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<List<TModel>>($"/v1/{_path}");

            return apiResponse;
        }

        public virtual async Task<TModel> Get(int id)
        {
            var apiResponse = await _httpClient.GetFromJsonAsync<TModel>($"/v1/{_path}/{id}");

            return apiResponse;
        }

        public async Task<TModel> Post(TModel model)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync($"/v1/{_path}", model);

            apiResponse.EnsureSuccessStatusCode();

            return await apiResponse.Content.ReadFromJsonAsync<TModel>();
        }

        public async Task<TModel> Put(TModel model, int id)
        {
            var apiResponse = await _httpClient.PutAsJsonAsync($"/v1/{_path}/{id}", model);

            apiResponse.EnsureSuccessStatusCode();

            return await apiResponse.Content.ReadFromJsonAsync<TModel>();
        }

        public async Task Delete(int id)
        {
            var apiResponse = await _httpClient.DeleteAsync($"/v1/{_path}/{id}");

            apiResponse.EnsureSuccessStatusCode();

        }

    }
}
