using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<List<Feedback>> GetList(int page, int size);
        public Task<Feedback?> Get(int? id);
        public Task<List<Feedback>> GetUserFeedbacks(string uid, int page, int size);
        public Task<Feedback?> GetSingleUserFeedback(int? id, string uid);
        public Task Add(Feedback obj);
        public Task Update(Feedback obj);
        public Task Delete(Feedback obj);
        public Task<int> CountFeedbacks(int? fieldId);
        public Task<List<Feedback>> GetFieldFeedbacks(int? fieldId);
    }
}
