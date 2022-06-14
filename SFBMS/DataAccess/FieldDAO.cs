using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FieldDAO
    {
        private static FieldDAO? instance = null;
        private static readonly object instanceLock = new();
        private FieldDAO() { }

        public static FieldDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FieldDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Field>> GetList(string? search, int page, int size)
        {
            var db = new SfbmsDbContext();
            IEnumerable<Field>? list = null;
            list = await db.Fields        
                .ToListAsync();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(f => f.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return list.Skip((page - 1) * size)
                    .Take(size); 
        }

        public async Task<List<Slot>> GetFieldSlotsByDate(int? fieldId, DateTime? date)
        {
            var db = new SfbmsDbContext();
            List<Slot>? list = null;
            list = await db.Slots
                .Where(x => x.FieldId == fieldId)
                .ToListAsync();

            if (date.HasValue)
            {
                list = list.Where(x => x.StartTime.Date.CompareTo(date.Value.Date) == 0).ToList();
            }
            return list;
        }

        public async Task<Field?> Get(int? id)
        {
            var db = new SfbmsDbContext();
            Field? obj = await db.Fields.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(Field obj)
        {
            var db = new SfbmsDbContext();
            db.Fields.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Field obj)
        {
            var db = new SfbmsDbContext();
            db.Fields.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Field obj)
        {
            var db = new SfbmsDbContext();
            db.Fields.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
