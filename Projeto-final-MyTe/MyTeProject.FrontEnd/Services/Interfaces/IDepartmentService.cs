using MyTeProject.FrontEnd.Models.Filters;
using MyTeProject.FrontEnd.Models.UserModels;

namespace MyTeProject.FrontEnd.Services.Interfaces
{
    public interface IDepartmentService : ICRUDService<DepartmentModel>
    {
        public Task<List<DepartmentModel>> Get(DepartmentFilter filter);
    }
}
