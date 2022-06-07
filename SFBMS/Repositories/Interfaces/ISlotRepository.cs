using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISlotRepository
    {
        public Task<List<Slot>> GetList();
        public Task<Slot?> Get(int id);
        public Task Add(Slot obj);
        public Task Update(Slot obj);
        public Task Delete(int id);
    }
}
