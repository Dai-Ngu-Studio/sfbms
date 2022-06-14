using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User?> Get(string id) => UserDAO.Instance.Get(id);
        public Task<List<User>> GetList() => UserDAO.Instance.GetList();
        public Task Add(User obj) => UserDAO.Instance.Add(obj);
        public Task Update(User obj) => UserDAO.Instance.Update(obj);
        public Task Delete(User obj) => UserDAO.Instance.Delete(obj);
    }
}
