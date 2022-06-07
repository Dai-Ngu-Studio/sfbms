using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("user_id")]
        public int FieldId { get; set; }

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
