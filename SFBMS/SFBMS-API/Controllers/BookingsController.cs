using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ODataController
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly IFieldRepository fieldRepository;

        public BookingsController(IBookingRepository _bookingRepository, IBookingDetailRepository _bookingDetailRepository, IFieldRepository _fieldRepository)
        {
            bookingRepository = _bookingRepository;
            bookingDetailRepository = _bookingDetailRepository;
            fieldRepository = _fieldRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Booking>>> Get()
        {
            return Ok(await bookingRepository.GetList(GetCurrentUID()));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Booking>> GetBooking(int key)
        {
            var obj = await bookingRepository.Get(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Booking not found");
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> Post(Booking obj)
        {
            try
            {
                if (obj.BookingDetails?.Count > 0)
                {
                    Booking booking = new Booking
                    {
                        UserId = GetCurrentUID(),
                    };
                    await bookingRepository.Add(booking);

                    BookingDetail bookingDetail = null;
                    decimal totalPrice = 0;
                    foreach (var item in obj.BookingDetails)
                    {
                        var field = await fieldRepository.Get(item.FieldId);
                        if (field == null)
                        {
                            return NotFound("Field not found");
                        }

                        bookingDetail = new BookingDetail
                        {
                            BookingId = booking.Id,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            FieldId = item.FieldId,
                            UserId = GetCurrentUID(),
                            SlotNumber = item.SlotNumber,
                            Price = field.Price,
                        };
                        await bookingDetailRepository.Add(bookingDetail);
                        totalPrice += bookingDetail.Price;
                    }

                    Booking _booking = new Booking
                    {
                        Id = booking.Id,
                        UserId = booking.UserId,
                        TotalPrice = totalPrice
                    };
                    await bookingRepository.Update(_booking);
                    return Created(_booking);
                }

                return BadRequest();
            }
            catch
            {
                if (await bookingRepository.Get(obj.Id, GetCurrentUID()) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        //[HttpPut("{key}")]
        //public async Task<ActionResult<Booking>> Put(int key, Booking obj)
        //{
        //    if (key != obj.Id)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        await bookingRepository.Update(obj);
        //        return Updated(obj);
        //    }
        //    catch
        //    {
        //        if (await bookingRepository.Get(obj.Id, GetCurrentUID()) == null)
        //        {
        //            return NotFound();
        //        }
        //        return BadRequest();
        //    }
        //}

        [HttpDelete("{key}")]
        public async Task<ActionResult<Booking>> Delete(int key)
        {
            var obj = await bookingRepository.Get(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Booking not found");
            }
            int userBookings = await bookingDetailRepository.CountBookingDetails(obj.Id);
            if (userBookings > 0)
            {
                return BadRequest();
            }

            try
            {
                await bookingRepository.Delete(obj);
                return NoContent();
            }
            catch
            {
                if (await bookingRepository.Get(key, GetCurrentUID()) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        private string GetCurrentUID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
