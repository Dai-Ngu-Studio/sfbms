using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("total_price", TypeName = "money")]
        public decimal TotalPrice { get; set; }

        [Column("user_id", TypeName = "varchar(128)")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
