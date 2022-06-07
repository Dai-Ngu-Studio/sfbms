using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SlotDAO
    {
        private static SlotDAO? instance = null;
        private static readonly object instanceLock = new();
        private SlotDAO() { }

        public static SlotDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SlotDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Slot>> GetList()
        {
            var db = new SfbmsDbContext();
            List<Slot>? list = null;
            list = await db.Slots.ToListAsync();
            return list;
        }

        public async Task<Slot?> Get(int id)
        {
            var db = new SfbmsDbContext();
            Slot? obj = await db.Slots.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(Slot obj)
        {
            var db = new SfbmsDbContext();
            db.Slots.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Slot obj)
        {
            var db = new SfbmsDbContext();
            db.Slots.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new SfbmsDbContext();
            Slot obj = new Slot { Id = id };
            db.Slots.Attach(obj);
            db.Slots.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
