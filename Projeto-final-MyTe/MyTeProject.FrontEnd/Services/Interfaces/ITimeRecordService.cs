using MyTeProject.FrontEnd.Models.TimeRecordModels;

namespace MyTeProject.FrontEnd.Services.Interfaces
{
    public interface ITimeRecordService
    {
        public Task<FortnightModel> Get(DateTime date);
        public Task<FortnightModel> Post(FortnightModel model);
    }
}
