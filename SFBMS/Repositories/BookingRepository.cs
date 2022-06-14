using BusinessObject;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public Task<Booking?> Get(int? id, string uid) => BookingDAO.Instance.Get(id, uid);
        public Task<List<Booking>> GetList(string uid) => BookingDAO.Instance.GetList(uid);
        public Task Add(Booking obj) => BookingDAO.Instance.Add(obj);
        public Task Update(Booking obj) => BookingDAO.Instance.Update(obj);
        public Task Delete(Booking obj) => BookingDAO.Instance.Delete(obj);
    }
}
