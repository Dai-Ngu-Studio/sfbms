using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Feedbacks = new HashSet<Feedback>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; } = "";

        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; } = "";

        [Column("name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = "";

        [Column("is_admin")]
        public int IsAdmin { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
