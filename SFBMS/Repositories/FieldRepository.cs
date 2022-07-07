using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class FieldRepository : IFieldRepository
    {
        public Task<Field?> Get(int? id) => FieldDAO.Instance.Get(id);
        public Task<IEnumerable<Field>> GetList() => FieldDAO.Instance.GetList();
        public Task Add(Field obj) => FieldDAO.Instance.Add(obj);
        public Task Update(Field obj) => FieldDAO.Instance.Update(obj);
        public Task Delete(Field obj) => FieldDAO.Instance.Delete(obj);
        public Task<int> GetTotalField(string search) => FieldDAO.Instance.GetTotalField(search);

        public Task<IEnumerable<Field>> GetListWithCategories(List<int> categoryIds) => FieldDAO.Instance.GetListWithCategories(categoryIds);
    }
}
