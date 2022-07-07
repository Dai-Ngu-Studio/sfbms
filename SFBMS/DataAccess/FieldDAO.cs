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

        public async Task<IEnumerable<Field>> GetList()
        {
            var db = new SfbmsDbContext();
            IEnumerable<Field>? list = await db.Fields
                .Include(x => x.Feedbacks)!
                .ThenInclude(x => x.User)
                .Include(c => c.Category)
                .Include(x => x.Slots)
                .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<Field>> GetListWithCategories(List<int> categoryIds)
        {
            var db = new SfbmsDbContext();
            IEnumerable<Field>? list = await db.Fields
                .Include(x => x.Feedbacks)!
                .ThenInclude(x => x.User)
                .Include(c => c.Category)
                .Include(x => x.Slots)
                .Where(x => categoryIds.Contains((int)x.CategoryId!))
                .Where(x => true)
                .ToListAsync();
            return list;
        }

        public async Task<int> GetTotalField(string search)
        {
            try
            {
                var db = new SfbmsDbContext();
                if (search != null)
                {
                    int TotalField = await db.Fields
                        .Where(field => field.Name!.ToLower().Contains(search))
                        .CountAsync();
                    return TotalField;
                }
                else
                {
                    int TotalField = await db.Fields.CountAsync();
                    return TotalField;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Field?> Get(int? id)
        {
            var db = new SfbmsDbContext();
            Field? obj = await db.Fields
                .Include(c => c.Category)
                .Include(x => x.Slots)
                .Include(x => x.Feedbacks)!
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
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
