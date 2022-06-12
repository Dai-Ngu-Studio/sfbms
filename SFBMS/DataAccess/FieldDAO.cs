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

        public async Task<List<Field>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Field>? list = null;
            list = await db.Fields.ToListAsync();
            return list;
        }

        public async Task<Field?> Get(int id)
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

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            Field obj = new Field { Id = id };
            db.Fields.Attach(obj);
            db.Fields.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
