using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class SlotRepository : ISlotRepository
    {
        public Task<Slot?> Get(int id) => SlotDAO.Instance.Get(id);
        public Task<List<Slot>> GetList() => SlotDAO.Instance.GetList();
        public Task Add(Slot obj) => SlotDAO.Instance.Add(obj);
        public Task Update(Slot obj) => SlotDAO.Instance.Update(obj);
        public Task Delete(int id) => SlotDAO.Instance.Delete(id);
    }
}
