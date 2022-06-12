using BusinessObject;

namespace Repositories.Interfaces
{
    public interface ISlotRepository
    {
        public Task<List<Slot>> GetList();
        public Task<Slot?> Get(int id);
        public Task Add(Slot obj);
        public Task Update(Slot obj);
        public Task Delete(int id);
    }
}
