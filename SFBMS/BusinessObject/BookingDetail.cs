using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class BookingDetail
    {
        public BookingDetail()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("booking_id")]
        public int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Column("start_time", TypeName = "datetime2(7)")]
        public DateTime StartTime { get; set; }

        [Column("end_time", TypeName = "datetime2(7)")]
        public DateTime EndTime { get; set; }

        [Column("field_id")]
        public int? FieldId { get; set; }

        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        [Column("user_id", TypeName = "varchar(128)")]
        public string? UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("slot_number")]
        public int SlotNumber { get; set; }

        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }

        [Column("status")]
        public int Status { get; set; }

        public HashSet<Feedback>? Feedbacks { get; set; }
    }

    public enum BookingDetailStatus
    {
        NotYet,
        Open,
        Attended,
        Absent
    }
}
