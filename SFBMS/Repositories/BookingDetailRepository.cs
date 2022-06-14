using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public Task<BookingDetail?> Get(int? id, string uid) => BookingDetailDAO.Instance.Get(id, uid);
        public Task<int> CountBookingDetails(int? bookingId) => BookingDetailDAO.Instance.CountBookingDetails(bookingId);
        public Task<List<BookingDetail>> GetList(string uid) => BookingDetailDAO.Instance.GetList(uid);
        public Task Add(BookingDetail obj) => BookingDetailDAO.Instance.Add(obj);
        public Task Update(BookingDetail obj) => BookingDetailDAO.Instance.Update(obj);
        public Task Delete(BookingDetail obj) => BookingDetailDAO.Instance.Delete(obj);
    }
}
