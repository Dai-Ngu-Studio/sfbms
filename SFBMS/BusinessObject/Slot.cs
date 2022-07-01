﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Slot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("field_id")]
        public int? FieldId { get; set; }

        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        [Column("start_time", TypeName = "datetime2(7)")]
        public DateTime StartTime { get; set; }

        [Column("end_time", TypeName = "datetime2(7)")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("slot_number")]
        public int SlotNumber { get; set; }

        public int? BookingStatus { get; set; } = 0;
    }
}
