using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class SlotRepository : ISlotRepository
    {
        public Task<Slot?> Get(int? id) => SlotDAO.Instance.Get(id);
        public Task<int> CountFieldSlots(int? fieldId) => SlotDAO.Instance.CountFieldSlots(fieldId);
        public Task<List<Slot>> GetList() => SlotDAO.Instance.GetList();
        public Task Add(Slot obj) => SlotDAO.Instance.Add(obj);
        public Task Update(Slot obj) => SlotDAO.Instance.Update(obj);
        public Task Delete(Slot obj) => SlotDAO.Instance.Delete(obj);
        public Task<List<Slot>> GetFieldSlotsByDate(int? fieldId, DateTime? date) => SlotDAO.Instance.GetFieldSlotsByDate(fieldId, date);
    }
}
