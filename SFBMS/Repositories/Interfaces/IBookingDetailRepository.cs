using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        public Task<List<BookingDetail>> GetList();
        public Task<BookingDetail?> Get(int id);
        public Task Add(BookingDetail obj);
        public Task Update(BookingDetail obj);
        public Task Delete(int id);
    }
}
