using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFieldRepository
    {
        public Task<List<Field>> GetList(string? search);
        public Task<Field?> Get(int? id);
        public Task Add(Field obj);
        public Task Update(Field obj);
        public Task Delete(Field obj);
        public Task<List<Slot>> GetFieldSlotsByDate(int? fieldId, DateTime? date);
    }
}
