using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    [Table("Field")]
    public class Field
    {
        public Field()
        {
            Slots = new HashSet<Slot>();
            Feedbacks = new HashSet<Feedback>();
            BookingDetails = new HashSet<BookingDetail>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = "";

        [Column("description", TypeName = "nvarchar(600)")]
        public string Description { get; set; } = "";

        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Column("number_of_slots")]
        public int NumberOfSlots { get; set; }

        public ICollection<Slot>? Slots { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<BookingDetail>? BookingDetails { get; set; }
    }
}
