using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<List<Feedback>> GetList(string uid);
        public Task<Feedback?> Get(int? id, string uid);
        public Task Add(Feedback obj);
        public Task Update(Feedback obj);
        public Task Delete(Feedback obj);
    }
}
