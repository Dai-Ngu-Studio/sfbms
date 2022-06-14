using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserDAO
    {
        private static UserDAO? instance = null;
        private static readonly object instanceLock = new();
        private UserDAO() { }

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<User>> GetList()
        {
            var db = new SfbmsDbContext();
            List<User>? list = null;
            list = await db.Users.ToListAsync();
            return list;
        }

        public async Task<User?> Get(string id)
        {
            var db = new SfbmsDbContext();
            User? obj = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(User obj)
        {
            var db = new SfbmsDbContext();
            db.Users.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(User obj)
        {
            var db = new SfbmsDbContext();
            db.Users.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(User obj)
        {
            var db = new SfbmsDbContext();
            db.Users.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
