using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFieldRepository
    {
        public Task<List<Field>> GetList();
        public Task<Field?> Get(int id);
        public Task Add(Field obj);
        public Task Update(Field obj);
        public Task Delete(int id);
    }
}
