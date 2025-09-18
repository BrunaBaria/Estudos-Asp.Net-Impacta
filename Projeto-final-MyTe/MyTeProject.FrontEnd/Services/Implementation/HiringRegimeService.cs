using MyTeProject.FrontEnd.Models.UserModels;
using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class HiringRegimeService : CRUDService<HiringRegimeModel>, IHiringRegimeService
    {

        public HiringRegimeService(HttpClient httpClient) : base(httpClient, "hiringRegime")
        {
        }
    }
}
