using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public Task<Booking?> Get(int id) => BookingDAO.Instance.Get(id);
        public Task<List<Booking>> GetList() => BookingDAO.Instance.GetList();
        public Task Add(Booking obj) => BookingDAO.Instance.Add(obj);
        public Task Update(Booking obj) => BookingDAO.Instance.Update(obj);
        public Task Delete(int id) => BookingDAO.Instance.Delete(id);
    }
}
