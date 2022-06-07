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

        public async Task<List<Booking>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Booking>? list = null;
            list = await db.Bookings.ToListAsync();
            return list;
        }

        public async Task<Booking?> Get(int id)
        {
            var db = new SfbmsDbContext();
            Booking? obj = await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            Booking obj = new Booking { Id = id };
            db.Bookings.Attach(obj);
            db.Bookings.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
