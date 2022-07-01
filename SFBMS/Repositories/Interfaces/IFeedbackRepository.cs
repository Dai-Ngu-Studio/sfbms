using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<IEnumerable<Feedback>> GetList();
        public Task<Feedback?> Get(int? id);
        public Task<List<Feedback>> GetUserFeedbacks(string uid);
        public Task<Feedback?> GetSingleUserFeedback(int? id, string uid);
        public Task Add(Feedback obj);
        public Task Update(Feedback obj);
        public Task Delete(Feedback obj);
        public Task<int> CountFeedbacks(int? fieldId);
        public Task<int> GetTotalFeedbacks();
        public Task<List<Feedback>> GetFieldFeedbacks(int? fieldId);
    }
}
