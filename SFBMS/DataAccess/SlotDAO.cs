using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SlotDAO
    {
        private static SlotDAO? instance = null;
        private static readonly object instanceLock = new();
        private SlotDAO() { }

        public static SlotDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SlotDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Slot>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Slot>? list = null;
            list = await db.Slots.ToListAsync();
            return list;
        }

        public async Task<Slot?> Get(int? id)
        {
            var db = new SfbmsDbContext();
            Slot? obj = await db.Slots.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task<int> CountFieldSlots(int? fieldId)
        {
            var db = new SfbmsDbContext();
            int fieldSlots = await db.Slots
                .Where(x => x.FieldId == fieldId)
                .CountAsync();
            return fieldSlots;
        }

        public async Task Add(Slot obj)
        {
            var db = new SfbmsDbContext();
            db.Slots.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Slot obj)
        {
            var db = new SfbmsDbContext();
            db.Slots.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Slot obj)
        {
            var db = new SfbmsDbContext();
            db.Slots.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
