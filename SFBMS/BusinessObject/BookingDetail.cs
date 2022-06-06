using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BookingDetail
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("booking_id")]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Column("start_time", TypeName = "datetime2(7)")]
        public DateTime StartTime { get; set; }

        [Column("end_time", TypeName = "datetime2(7)")]
        public DateTime EndTime { get; set; }

        [Column("field_id")]
        public int FieldId { get; set; }

        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("slot_number")]
        public int SlotNumber { get; set; }

        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }
    }
}
