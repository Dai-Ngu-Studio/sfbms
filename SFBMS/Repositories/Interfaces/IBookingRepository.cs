using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingRepository
    {
        public Task<List<Booking>> GetList();
        public Task<Booking?> Get(int id);
        public Task Add(Booking obj);
        public Task Update(Booking obj);
        public Task Delete(int id);
    }
}
