using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<Category?> Get(int? id) => CategoryDAO.Instance.Get(id);
        public Task<List<Category>> GetList() => CategoryDAO.Instance.GetList();
        public Task Add(Category obj) => CategoryDAO.Instance.Add(obj);
        public Task Update(Category obj) => CategoryDAO.Instance.Update(obj);
        public Task Delete(Category obj) => CategoryDAO.Instance.Delete(obj);
    }
}
