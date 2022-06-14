using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingRepository
    {
        public Task<List<Booking>> GetList(string uid);
        public Task<Booking?> Get(int? id, string uid);
        public Task Add(Booking obj);
        public Task Update(Booking obj);
        public Task Delete(Booking obj);
    }
}
