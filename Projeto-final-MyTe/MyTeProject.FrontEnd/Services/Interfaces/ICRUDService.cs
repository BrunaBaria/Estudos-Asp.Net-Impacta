namespace MyTeProject.FrontEnd.Services.Interfaces
{
    public interface ICRUDService<TModel> where TModel : class
    {
        public Task<List<TModel>> Get();
        public Task<TModel> Get(int id);
        public Task<TModel> Post(TModel model);
        public Task<TModel> Put(TModel model, int id);
        public Task Delete(int id);
    }
}
