using BusinessObject;

namespace Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        public Task<IEnumerable<BookingDetail>> GetUserList(string uid);
        public Task<IEnumerable<BookingDetail>> GetAdminList();
        public Task<int> GetTotalBookingDetail();
        public Task<int> CountBookingDetails(int? bookingId);
        public Task<BookingDetail?> GetUserBookingDetail(int? id, string uid);
        public Task<BookingDetail?> GetBookingDetailForAdmin(int? id);
        public Task Add(BookingDetail obj);
        public Task Update(BookingDetail obj);
        public Task Delete(BookingDetail obj);
        public Task<IEnumerable<BookingDetail>> GetBookingDetailsForDate(int fieldId, DateTime bookingDate);
    }
}
