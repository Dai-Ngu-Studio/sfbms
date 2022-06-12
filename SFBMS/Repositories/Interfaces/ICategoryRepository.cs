using BusinessObject;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetList();
        public Task<Category?> Get(int id);
        public Task Add(Category obj);
        public Task Update(Category obj);
        public Task Delete(int id);
    }
}
