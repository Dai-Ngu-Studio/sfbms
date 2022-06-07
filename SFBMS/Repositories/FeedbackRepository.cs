using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public Task<Feedback?> Get(int id) => FeedbackDAO.Instance.Get(id);
        public Task<List<Feedback>> GetList() => FeedbackDAO.Instance.GetList();
        public Task Add(Feedback obj) => FeedbackDAO.Instance.Add(obj);
        public Task Update(Feedback obj) => FeedbackDAO.Instance.Update(obj);
        public Task Delete(int id) => FeedbackDAO.Instance.Delete(id);
    }
}
