using MyTeProject.FrontEnd.Models.WBSModels;
using MyTeProject.FrontEnd.Services.Abstract;
using MyTeProject.FrontEnd.Services.Interfaces;

namespace MyTeProject.FrontEnd.Services.Implementation
{
    public class WBSTypeService : CRUDService<WBSTypeModel>, IWBSTypeService
    {
        public WBSTypeService(HttpClient httpClient) : base(httpClient, "wbsType")
        {
        }
    }
}
