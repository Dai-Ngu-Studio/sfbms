using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFieldRepository
    {
        public Task<IEnumerable<Field>> GetList();
        public Task<Field?> Get(int? id);
        public Task Add(Field obj);
        public Task Update(Field obj);
        public Task Delete(Field obj);
        public Task<int> GetTotalField(string search);
    }
}
