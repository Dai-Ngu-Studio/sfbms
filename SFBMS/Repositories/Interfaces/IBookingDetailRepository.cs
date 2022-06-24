using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        public Task<IEnumerable<BookingDetail>> GetUserList(string uid, int page, int size);
        public Task<IEnumerable<BookingDetail>> GetAdminList(int page, int size);
        public Task<int> GetTotalBookingDetail();
        public Task<int> CountBookingDetails(int? bookingId);
        public Task<BookingDetail?> Get(int? id, string uid);
        public Task Add(BookingDetail obj);
        public Task Update(BookingDetail obj);
        public Task Delete(BookingDetail obj);
    }
}
