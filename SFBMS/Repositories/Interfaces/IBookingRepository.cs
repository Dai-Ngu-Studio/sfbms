using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingRepository
    {
        public Task<List<Booking>> GetList();
        public Task<Booking?> Get(int id);
        public Task Add(Booking obj);
        public Task Update(Booking obj);
        public Task Delete(int id);
    }
}
