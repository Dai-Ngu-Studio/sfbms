using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO? instance = null;
        private static readonly object instanceLock = new();
        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Category>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Category>? list = null;
            list = await db.Categories.ToListAsync();
            return list;
        }

        public async Task<Category?> Get(int id)
        {
            var db = new SfbmsDbContext();
            Category? obj = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(Category obj)
        {
            var db = new SfbmsDbContext();
            db.Categories.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Category obj)
        {
            var db = new SfbmsDbContext();
            db.Categories.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            Category obj = new Category { Id = id };
            db.Categories.Attach(obj);
            db.Categories.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
