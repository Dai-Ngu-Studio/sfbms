using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        public int? Status { get; set; }

        [Column("slot_number")]
        public int SlotNumber { get; set; }

        public int? BookingStatus { get; set; }

        //[NotMapped]
        //[JsonIgnore]
        //[IgnoreDataMember]
        //public static Dictionary<int, DateTime> BookingTimes = new Dictionary<int, DateTime>
        //{
        //    {
        //        (int)BookingTime.Morning, new DateTime(0001, 1, 1, 6, 0, 0)
        //    },
        //    {
        //        (int)BookingTime.Noon, new DateTime(0001, 1, 1, 10, 0, 0)
        //    },
        //    {
        //        (int)BookingTime.Afternoon, new DateTime(0001, 1, 1, 14, 0, 0)
        //    },
        //    {
        //        (int)BookingTime.Evening, new DateTime(0001, 1, 1, 18, 0, 0)
        //    },
        //    {
        //        (int)BookingTime.Night, new DateTime(0001, 1, 1, 22, 0, 0)
        //    },
        //    {
        //        (int)BookingTime.Midnight, new DateTime(0001, 1, 1, 2, 0, 0)
        //    }
        //};
    }

    //public enum BookingTime
    //{
    //    // 06:00 - 10:00
    //    Morning,
    //    // 10:00 - 14:00
    //    Noon,
    //    // 14:00 - 18:00
    //    Afternoon,
    //    // 18:00 - 22:00
    //    Evening,
    //    // 22:00 - 02:00
    //    Night,
    //    // 02:00 - 06:00
    //    Midnight
    //}
}
