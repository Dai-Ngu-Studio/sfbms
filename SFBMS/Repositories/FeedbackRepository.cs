﻿using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public Task<Feedback?> Get(int? id) => FeedbackDAO.Instance.Get(id);
        public Task<IEnumerable<Feedback>> GetList() => FeedbackDAO.Instance.GetList();
        public Task Add(Feedback obj) => FeedbackDAO.Instance.Add(obj);
        public Task Update(Feedback obj) => FeedbackDAO.Instance.Update(obj);
        public Task Delete(Feedback obj) => FeedbackDAO.Instance.Delete(obj);
        public Task<List<Feedback>> GetUserFeedbacks(string uid) => FeedbackDAO.Instance.GetUserFeedbacks(uid);
        public Task<Feedback?> GetSingleUserFeedback(int? id, string uid) => FeedbackDAO.Instance.GetSingleUserFeedback(id, uid);
        public Task<int> CountFeedbacks(int? fieldId) => FeedbackDAO.Instance.CountFeedbacks(fieldId);
        public Task<int> GetTotalFeedbacks() => FeedbackDAO.Instance.GetTotalFeedbacks();
        public Task<List<Feedback>> GetFieldFeedbacks(int? fieldId) => FeedbackDAO.Instance.GetFieldFeedbacks(fieldId);
    }
}
