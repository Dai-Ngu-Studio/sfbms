using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
