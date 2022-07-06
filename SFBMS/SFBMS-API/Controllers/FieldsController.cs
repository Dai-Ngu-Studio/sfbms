using BusinessObject;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;
using System.Globalization;
using System.Security.Claims;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FieldsController : ODataController
    {
        private readonly IFieldRepository fieldRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISlotRepository slotRepository;
        private readonly IUserRepository userRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;

        public FieldsController(IFieldRepository _fieldRepository, ICategoryRepository _categoryRepository, ISlotRepository _slotRepository,
            IUserRepository _userRepository, IBookingDetailRepository _bookingDetailRepository)
        {
            fieldRepository = _fieldRepository;
            categoryRepository = _categoryRepository;
            slotRepository = _slotRepository;
            userRepository = _userRepository;
            bookingDetailRepository = _bookingDetailRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Field>>> Get()
        {
            var fieldList = await fieldRepository.GetList();
            return Ok(fieldList);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Field>> GetField(int key)
        {
            var obj = await fieldRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Field not found");
            }
            return Ok(obj);
        }

        [EnableQuery]
        [HttpPost("{key}/slot-status")]
        public async Task<ActionResult<Field>> SlotStatus([FromODataUri] int key, ODataActionParameters parameters)
        {
            try
            {
                var obj = await fieldRepository.Get(key);
                if (obj == null)
                {
                    return NotFound("Field not found");
                }

                var bookingDate = (DateTimeOffset)parameters["BookingDate"];
                //var _bookingDate = DateTime.Parse(bookingDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                List<BookingDetail> bookingDetails = (await bookingDetailRepository.GetBookingDetailsForDate(key, bookingDate.DateTime)).ToList();

                foreach (var slot in obj.Slots!)
                {
                    slot.BookingStatus = bookingDetails.Any(x => x.StartTime.TimeOfDay == slot.StartTime.TimeOfDay) ? 1 : 0;
                    slot.StartTime = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, 
                        slot.StartTime.Hour, slot.StartTime.Minute, slot.StartTime.Second, slot.StartTime.Millisecond, DateTimeKind.Local);
                    slot.EndTime = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day,
                        slot.EndTime.Hour, slot.EndTime.Minute, slot.StartTime.Second, slot.EndTime.Millisecond, DateTimeKind.Local);
                }
                return Ok(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Field>> Post(Field obj)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var category = await categoryRepository.Get(obj.CategoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }

                try
                {
                    if (obj.NumberOfSlots == 8 || obj.NumberOfSlots == 12 || obj.NumberOfSlots == 16 || obj.NumberOfSlots == 18 
                            || obj.NumberOfSlots == 22 || obj.NumberOfSlots == 24)
                    {
                        Field field = new Field
                        {
                            CategoryId = obj.CategoryId,
                            Name = obj.Name,
                            Description = obj.Description,
                            Price = obj.Price < 0 ? 10000 : obj.Price,
                            NumberOfSlots = obj.NumberOfSlots,
                            TotalRating = 0,
                            ImageUrl = obj.ImageUrl
                        };
                        await fieldRepository.Add(field);

                        int numberOfMinutesPerDay = 1440;
                        int minutesPerSlot = numberOfMinutesPerDay / obj.NumberOfSlots;

                        CalendarDateAdd calendarDateAdd = new CalendarDateAdd();
                        DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
                        TimeSpan offset = TimeSpan.FromMinutes(minutesPerSlot);
                        DateTime? end = calendarDateAdd.Add(start, offset);
                        TimeSpan interval = new TimeSpan(0, 0, 0);

                        for (int i = 1; i <= obj.NumberOfSlots; i++)
                        {
                            int slotNumbers = await slotRepository.CountFieldSlots(field.Id);
                            Slot slot = new Slot
                            {
                                FieldId = field.Id,
                                StartTime = start,
                                EndTime = end!.Value,
                                Status = 0,
                                SlotNumber = slotNumbers + 1,
                            };
                            await slotRepository.Add(slot);
                            start = (DateTime)calendarDateAdd.Add(end.Value, interval)!;
                            end = calendarDateAdd.Add(start, offset);
                        }
                        return Created(field);
                    }
                    return BadRequest();
                }
                catch
                {
                    if (await fieldRepository.Get(obj.Id) != null)
                    {
                        return Conflict();
                    }
                    return BadRequest();
                }
            }
            return Unauthorized();
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Field>> Put(int key, Field obj)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var currentField = await fieldRepository.Get(key);
                if (currentField == null)
                {
                    return NotFound("Field not found");
                }

                try
                {
                    Field field = new Field
                    {
                        Id = currentField.Id,
                        CategoryId = obj.CategoryId == null ? currentField.CategoryId : obj.CategoryId,
                        Name = obj.Name == null ? currentField.Name : obj.Name,
                        Description = obj.Description == null ? currentField.Description : obj.Description,
                        ImageUrl = obj.ImageUrl == null ? currentField.ImageUrl : obj.ImageUrl,
                        Price = obj.Price <= 0 ? currentField.Price : obj.Price,
                        NumberOfSlots = currentField.NumberOfSlots,
                        TotalRating = currentField.TotalRating,
                    };

                    await fieldRepository.Update(field);
                    return Updated(obj);
                }
                catch
                {
                    if (await fieldRepository.Get(obj.Id) == null)
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return Unauthorized();
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Field>> Delete(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var field = await fieldRepository.Get(key);
                if (field == null)
                {
                    return NotFound("Field not found");
                }
                int fieldSlots = await slotRepository.CountFieldSlots(key);
                if (fieldSlots > 0)
                {
                    return BadRequest("Please delete all slots from this field before attempting to delete");
                }

                try
                {
                    await fieldRepository.Delete(field);
                    return NoContent();
                }
                catch
                {
                    if (await fieldRepository.Get(key) == null)
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return Unauthorized();
        }
        private string GetCurrentUID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
