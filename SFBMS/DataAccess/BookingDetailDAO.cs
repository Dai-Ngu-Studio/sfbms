using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<BookingDetail>> GetList()
        {
            var db = new SfbmsDbContext();
            List<BookingDetail>? list = null;
            list = await db.BookingDetails.ToListAsync();
            return list;
        }

        public async Task<BookingDetail?> Get(int id)
        {
            var db = new SfbmsDbContext();
            BookingDetail? obj = await db.BookingDetails.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
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

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            BookingDetail obj = new BookingDetail { Id = id };
            db.BookingDetails.Attach(obj);
            db.BookingDetails.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
