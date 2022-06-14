using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public Task<Feedback?> Get(int? id, string uid) => FeedbackDAO.Instance.Get(id, uid);
        public Task<List<Feedback>> GetList(string uid) => FeedbackDAO.Instance.GetList(uid);
        public Task Add(Feedback obj) => FeedbackDAO.Instance.Add(obj);
        public Task Update(Feedback obj) => FeedbackDAO.Instance.Update(obj);
        public Task Delete(Feedback obj) => FeedbackDAO.Instance.Delete(obj);
    }
}
