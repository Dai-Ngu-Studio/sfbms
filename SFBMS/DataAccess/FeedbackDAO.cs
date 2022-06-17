using BusinessObject;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Feedback>> GetList(int page, int size)
        {
            var db = new SfbmsDbContext();
            List<Feedback>? list = null;
            list = await db.Feedbacks
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return list;
        }

        public async Task<Feedback?> Get(int? id)
        {
            var db = new SfbmsDbContext();
            Feedback? obj = await db.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }
        public async Task<int> CountFeedbacks(int? fieldId)
        {
            var db = new SfbmsDbContext();
            int feedbackCounts = await db.Feedbacks
                .Where(x => x.FieldId == fieldId)
                .CountAsync();
            return feedbackCounts;
        }

        public async Task<List<Feedback>> GetFieldFeedbacks(int? fieldId)
        {
            var db = new SfbmsDbContext();
            List<Feedback>? list = null;
            list = await db.Feedbacks
                .Where(x => x.FieldId == fieldId)
                .ToListAsync();
            return list;
        }

        public async Task<List<Feedback>> GetUserFeedbacks(string uid, int page, int size)
        {
            var db = new SfbmsDbContext();
            List<Feedback>? list = null;
            list = await db.Feedbacks
                .Where(x => x.UserId == uid)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return list;
        }

        public async Task<Feedback?> GetSingleUserFeedback(int? id, string uid)
        {
            var db = new SfbmsDbContext();
            Feedback? obj = await db.Feedbacks
                .FirstOrDefaultAsync(x => x.UserId == uid && x.Id == id);
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

        public async Task Delete(Feedback obj)
        {
            var db = new SfbmsDbContext();
            db.Feedbacks.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
