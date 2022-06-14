using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        public Task<List<BookingDetail>> GetList(string uid);
        public Task<int> CountBookingDetails(int? bookingId);
        public Task<BookingDetail?> Get(int? id, string uid);
        public Task Add(BookingDetail obj);
        public Task Update(BookingDetail obj);
        public Task Delete(BookingDetail obj);
    }
}
