using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BookingDetailDAO
    {
        private static BookingDetailDAO? instance = null;
        private static readonly object instanceLock = new();
        private BookingDetailDAO() { }

        public static BookingDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<BookingDetail>> GetUserList(string uid, int page, int size)
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = null;
            list = await db.BookingDetails
                .Where(x => x.UserId == uid)
                .ToListAsync();

            return list.Skip((page - 1) * size)
                    .Take(size);
        }
        public async Task<IEnumerable<BookingDetail>> GetAdminList(int page, int size)
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = null;
            list = await db.BookingDetails
                .Include(x => x.User)
                .Include(x => x.Field)
                .ToListAsync();

            return list.Skip((page - 1) * size)
                    .Take(size);
        }

        public async Task<int> GetTotalBookingDetail()
        {
            try
            {
                var db = new SfbmsDbContext();
                int TotalBookingDetail = await db.BookingDetails.CountAsync();
                return TotalBookingDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookingDetail?> GetUserBookingDetail(int? id, string uid)
        {
            var db = new SfbmsDbContext();
            BookingDetail? obj = await db.BookingDetails.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            return obj;
        }
        public async Task<BookingDetail?> GetBookingDetailForAdmin(int? id)
        {
            var db = new SfbmsDbContext();
            BookingDetail? obj = await db.BookingDetails.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task<int> CountBookingDetails(int? bookingId)
        {
            var db = new SfbmsDbContext();
            int userBookingCount = await db.BookingDetails
                .Where(x => x.BookingId == bookingId)
                .CountAsync();
            return userBookingCount;
        }

        public async Task Add(BookingDetail obj)
        {
            var db = new SfbmsDbContext();
            db.BookingDetails.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(BookingDetail obj)
        {
            var db = new SfbmsDbContext();
            db.BookingDetails.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(BookingDetail obj)
        {
            var db = new SfbmsDbContext();
            db.BookingDetails.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
