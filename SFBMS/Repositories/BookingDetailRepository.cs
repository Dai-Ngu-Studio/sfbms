using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public Task<BookingDetail?> Get(int id) => BookingDetailDAO.Instance.Get(id);
        public Task<List<BookingDetail>> GetList() => BookingDetailDAO.Instance.GetList();
        public Task Add(BookingDetail obj) => BookingDetailDAO.Instance.Add(obj);
        public Task Update(BookingDetail obj) => BookingDetailDAO.Instance.Update(obj);
        public Task Delete(int id) => BookingDetailDAO.Instance.Delete(id);
    }
}
