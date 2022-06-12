using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetList();
        public Task<User?> Get(string id);
        public Task Add(User obj);
        public Task Update(User obj);
        public Task Delete(string id);
    }
}
