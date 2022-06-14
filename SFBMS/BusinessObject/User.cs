using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        [Column("id", TypeName = "varchar(128)")]
        public string Id { get; set; } = null!;

        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; } = null!;

        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; } = null!;

        [Column("name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = null!;

        [Column("is_admin")]
        public int IsAdmin { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
