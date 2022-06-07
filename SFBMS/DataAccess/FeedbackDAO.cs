using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FeedbackDAO
    {
        private static FeedbackDAO? instance = null;
        private static readonly object instanceLock = new();
        private FeedbackDAO() { }

        public static FeedbackDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FeedbackDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Feedback>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Feedback>? list = null;
            list = await db.Feedbacks.ToListAsync();
            return list;
        }

        public async Task<Feedback?> Get(int id)
        {
            var db = new SfbmsDbContext();
            Feedback? obj = await db.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(Feedback obj)
        {
            var db = new SfbmsDbContext();
            db.Feedbacks.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Feedback obj)
        {
            var db = new SfbmsDbContext();
            db.Feedbacks.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            Feedback obj = new Feedback { Id = id };
            db.Feedbacks.Attach(obj);
            db.Feedbacks.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
