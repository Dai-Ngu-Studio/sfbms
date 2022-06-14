using BusinessObject;

namespace SFBMS_API.BusinessModels
{
    public class BookingModel
    {
        public ICollection<Slot>? Slots { get; set; }
    }
}
