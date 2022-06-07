using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        public Task<List<BookingDetail>> GetList();
        public Task<BookingDetail?> Get(int id);
        public Task Add(BookingDetail obj);
        public Task Update(BookingDetail obj);
        public Task Delete(int id);
    }
}
