﻿using System.ComponentModel.DataAnnotations;
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

        [Column("booking_detail_id")]
        public int? BookingDetailId { get; set; }

        [ForeignKey("BookingDetailId")]
        public BookingDetail? BookingDetail { get; set; }

        [Column("title", TypeName = "nvarchar(255)")]
        public string Title { get; set; } = "";

        [Column("content", TypeName = "nvarchar(800)")]
        public string Content { get; set; } = "";

        [Column("rating")]
        public int Rating { get; set; }

        [Column("feedback_time", TypeName = "datetime2(7)")]
        public DateTime? FeedbackTime { get; set; }

    }
}
