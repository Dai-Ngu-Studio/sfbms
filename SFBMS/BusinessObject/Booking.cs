using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("total_price", TypeName = "money")]
        public decimal TotalPrice { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
