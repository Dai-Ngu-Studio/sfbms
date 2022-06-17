using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFieldRepository
    {
        public Task<IEnumerable<Field>> GetList(string? search, int page, int size);
        public Task<Field?> Get(int? id);
        public Task Add(Field obj);
        public Task Update(Field obj);
        public Task Delete(Field obj);       
    }
}
