using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<List<Feedback>> GetList();
        public Task<Feedback?> Get(int id);
        public Task Add(Feedback obj);
        public Task Update(Feedback obj);
        public Task Delete(int id);
    }
}
