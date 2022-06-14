using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class FieldRepository : IFieldRepository
    {
        public Task<Field?> Get(int? id) => FieldDAO.Instance.Get(id);
        public Task<List<Field>> GetList() => FieldDAO.Instance.GetList();
        public Task Add(Field obj) => FieldDAO.Instance.Add(obj);
        public Task Update(Field obj) => FieldDAO.Instance.Update(obj);
        public Task Delete(Field obj) => FieldDAO.Instance.Delete(obj);
    }
}
