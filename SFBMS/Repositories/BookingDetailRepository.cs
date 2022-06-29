using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public Task<BookingDetail?> GetUserBookingDetail(int? id, string uid) => BookingDetailDAO.Instance.GetUserBookingDetail(id, uid);
        public Task<BookingDetail?> GetBookingDetailForAdmin(int? id) => BookingDetailDAO.Instance.GetBookingDetailForAdmin(id);
        public Task<int> CountBookingDetails(int? bookingId) => BookingDetailDAO.Instance.CountBookingDetails(bookingId);
        public Task<int> GetTotalBookingDetail() => BookingDetailDAO.Instance.GetTotalBookingDetail();
        public Task<IEnumerable<BookingDetail>> GetUserList(string uid, int page, int size) => BookingDetailDAO.Instance.GetUserList(uid, page, size);
        public Task<IEnumerable<BookingDetail>> GetAdminList(int page, int size) => BookingDetailDAO.Instance.GetAdminList(page, size);
        public Task Add(BookingDetail obj) => BookingDetailDAO.Instance.Add(obj);
        public Task Update(BookingDetail obj) => BookingDetailDAO.Instance.Update(obj);
        public Task Delete(BookingDetail obj) => BookingDetailDAO.Instance.Delete(obj);
    }
}
