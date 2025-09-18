using MyTeProject.FrontEnd.Models.TimeRecordModels;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class TimeRecordService : ITimeRecordService
    {
        protected readonly HttpClient _httpClient;
        protected const string Uri = "http://localhost:36881";
        protected readonly string _path = "timeRecord";

        public TimeRecordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Uri);
        }

        public async Task<FortnightModel> Get(DateTime date)
        {
            string formatedDate = date.ToString("MM-dd-yyyy");
            var apiResponse = await _httpClient.GetFromJsonAsync<FortnightModel>($"/v1/{_path}/{formatedDate}");

            return apiResponse;
        }

        public async Task<FortnightModel> Post(FortnightModel model)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync($"/v1/{_path}", model);

            apiResponse.EnsureSuccessStatusCode();

            return await apiResponse.Content.ReadFromJsonAsync<FortnightModel>();
        }

    }
}
