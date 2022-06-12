using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id", TypeName = "varchar(128)")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("field_id")]
        public int? FieldId { get; set; }

        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        [Column("title", TypeName = "nvarchar(255)")]
        public string Title { get; set; } = "";

        [Column("content", TypeName = "nvarchar(800)")]
        public string Content { get; set; } = "";

        [Column("rating")]
        public int Rating { get; set; }

    }
}
