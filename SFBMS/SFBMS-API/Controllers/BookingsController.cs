using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;
using SFBMS_API.BusinessModels;
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
        private readonly ISlotRepository slotRepository;
        private readonly IUserRepository userRepository;

        public BookingsController(IBookingRepository _bookingRepository, IBookingDetailRepository _bookingDetailRepository,
            IFieldRepository _fieldRepository, ISlotRepository _slotRepository, IUserRepository _userRepository)
        {
            bookingRepository = _bookingRepository;
            bookingDetailRepository = _bookingDetailRepository;
            fieldRepository = _fieldRepository;
            slotRepository = _slotRepository;
            userRepository = _userRepository;
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
        public async Task<ActionResult<Booking>> Post(BookingModel bookingModel)
        {
            try
            {
                if (bookingModel.Slots?.Count > 0)
                {
                    Booking booking = new Booking
                    {
                        UserId = GetCurrentUID(),
                    };
                    await bookingRepository.Add(booking);

                    decimal totalPrice = 0;
                    foreach (var item in bookingModel.Slots)
                    {
                        var field = await fieldRepository.Get(item.FieldId);
                        var slot = await slotRepository.Get(item.Id);

                        if (slot != null && field != null && item.Status == 0)
                        {
                            BookingDetail bookingDetail = new BookingDetail
                            {
                                BookingId = booking.Id,
                                StartTime = slot.StartTime,
                                EndTime = slot.EndTime,
                                FieldId = field.Id,
                                UserId = GetCurrentUID(),
                                SlotNumber = slot.SlotNumber,
                                Price = field.Price,
                                Status = 0
                            };
                            await bookingDetailRepository.Add(bookingDetail);
                            totalPrice += bookingDetail.Price;

                            Slot _slot = new Slot
                            {
                                Id = slot.Id,
                                SlotNumber = slot.SlotNumber,
                                StartTime = slot.StartTime,
                                EndTime = slot.EndTime,
                                FieldId = field.Id,
                                Status = 1
                            };
                            await slotRepository.Update(_slot);
                        }
                    }

                    int countUserBookingDetail = await bookingDetailRepository.CountBookingDetails(booking.Id);
                    if (countUserBookingDetail == 0)
                    {
                        await bookingRepository.Delete(booking);
                        return BadRequest("There are currently no booking detail for this user");
                    }

                    Booking _booking = new Booking
                    {
                        Id = booking.Id,
                        UserId = booking.UserId,
                        TotalPrice = totalPrice,
                        BookingDate = DateTime.Now,
                    };
                    await bookingRepository.Update(_booking);
                    return Created(_booking);
                }

                return BadRequest();
            }
            catch
            {
                //if (await bookingRepository.Get(bookingModel.Id, GetCurrentUID()) != null)
                //{
                //    return Conflict();
                //}
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
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
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
            return Unauthorized();
        }

        private string GetCurrentUID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
    }
}
