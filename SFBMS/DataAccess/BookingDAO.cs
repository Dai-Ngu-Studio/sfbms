using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BookingDAO
    {
        private static BookingDAO? instance = null;
        private static readonly object instanceLock = new();
        private BookingDAO() { }

        public static BookingDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Booking>> GetList(string uid)
        {
            var db = new SfbmsDbContext();
            List<Booking>? list = null;
            list = await db.Bookings
                .Where(x => x.UserId == uid)
                .ToListAsync();
            return list;
        }

        public async Task<Booking?> Get(int? id, string uid)
        {
            var db = new SfbmsDbContext();
            Booking? obj = await db.Bookings
                .Include(x => x.BookingDetails).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            return obj;
        }

        public async Task Add(Booking obj)
        {
            var db = new SfbmsDbContext();
            db.Bookings.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Booking obj)
        {
            var db = new SfbmsDbContext();
            db.Bookings.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Booking obj)
        {
            var db = new SfbmsDbContext();
            db.Bookings.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
