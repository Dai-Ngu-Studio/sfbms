using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetList();
        public Task<Category?> Get(int id);
        public Task Add(Category obj);
        public Task Update(Category obj);
        public Task Delete(int id);
    }
}
