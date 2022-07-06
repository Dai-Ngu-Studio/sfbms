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

        public async Task<IEnumerable<BookingDetail>> GetUserList(string uid)
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = await db.BookingDetails
                .Where(x => x.UserId == uid)
                .Include(x => x.Field)
                .ToListAsync();

            return list;
        }
        public async Task<IEnumerable<BookingDetail>> GetAdminList()
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = await db.BookingDetails
                .Include(x => x.User)
                .Include(x => x.Field)
                .ToListAsync();

            return list;
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
            BookingDetail? obj = await db.BookingDetails
                .Include(x => x.Field)
                .Include(x => x.Feedbacks)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
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

        public async Task<IEnumerable<BookingDetail>> GetBookingDetailsForDate(int fieldId, DateTime bookingDate)
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = await db.BookingDetails
                .Include(x => x.User)
                .Include(x => x.Field)
                .Where(x => x.StartTime.Date == bookingDate.Date && x.Field!.Id == fieldId)
                .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<BookingDetail>> GetPendingBookingDetailsForDate(DateTime date)
        {
            var db = new SfbmsDbContext();
            IEnumerable<BookingDetail>? list = await db.BookingDetails
                .Include(x => x.User)
                .Include(x => x.Field)
                .Where(x => x.StartTime.Date == date.Date 
                && (x.Status == (int)BookingDetailStatus.NotYet 
                || x.Status == (int)BookingDetailStatus.Open))
                .ToListAsync();

            return list;
        }

        public async Task UpdateRange(BookingDetail[] obj)
        {
            var db = new SfbmsDbContext();
            db.BookingDetails.UpdateRange(obj);
            await db.SaveChangesAsync();
        }
    }
}
