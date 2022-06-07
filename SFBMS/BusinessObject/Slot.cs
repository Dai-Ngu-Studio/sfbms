using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Slot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("field_id")]
        public int FieldId { get; set; }

        [ForeignKey("fieldId")]
        public Field? Field { get; set; }

        [Column("start_time", TypeName = "datetime2(7)")]
        public DateTime StartTime { get; set; }

        [Column("end_time", TypeName = "datetime2(7)")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}
