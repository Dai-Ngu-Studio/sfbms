using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingDetailsController : ODataController
    {
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly IUserRepository userRepository;

        public BookingDetailsController(IBookingDetailRepository _bookingDetailRepository, IUserRepository _userRepository)
        {
            bookingDetailRepository = _bookingDetailRepository;
            userRepository = _userRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<BookingDetail>>> Get()
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var list = await bookingDetailRepository.GetAdminList();
                return Ok(list);
            }
            return Ok(await bookingDetailRepository.GetUserList(GetCurrentUID()));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<BookingDetail>> GetBookingDetail(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var admin = await bookingDetailRepository.GetBookingDetailForAdmin(key);
                if (admin == null)
                {
                    return NotFound();
                }
                return Ok(admin);
            }
            var obj = await bookingDetailRepository.GetUserBookingDetail(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        //[HttpPost]
        //public async Task<ActionResult<BookingDetail>> Post(BookingDetail obj)
        //{
        //    try
        //    {
        //        await bookingDetailRepository.Add(obj);
        //        return Created(obj);
        //    }
        //    catch
        //    {
        //        if (await bookingDetailRepository.Get(obj.Id, GetCurrentUID()) != null)
        //        {
        //            return Conflict();
        //        }
        //        return BadRequest();
        //    }
        //}

        [HttpPut("{key}")]
        public async Task<ActionResult<BookingDetail>> Put(int key, BookingDetail obj)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var currentBookingDetail = await bookingDetailRepository.GetBookingDetailForAdmin(key);
                if (currentBookingDetail == null)
                {
                    return NotFound();
                }

                try
                {
                    BookingDetail bookingDetail = new BookingDetail
                    {
                        Id = currentBookingDetail.Id,
                        StartTime = currentBookingDetail.StartTime,
                        EndTime = currentBookingDetail.EndTime,
                        FieldId = currentBookingDetail.FieldId,
                        UserId = currentBookingDetail.UserId,
                        SlotNumber = currentBookingDetail.SlotNumber,
                        BookingId = currentBookingDetail.BookingId,
                        Price = currentBookingDetail.Price,
                        Status = obj.Status < 0 || obj.Status > 3 ? currentBookingDetail.Status : obj.Status
                    };

                    await bookingDetailRepository.Update(bookingDetail);
                    return Updated(bookingDetail);
                }
                catch
                {
                    if (await bookingDetailRepository.GetBookingDetailForAdmin(obj.Id) == null)
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return Unauthorized();
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<BookingDetail>> Delete(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var obj = await bookingDetailRepository.GetBookingDetailForAdmin(key);
                if (obj == null)
                {
                    return NotFound();
                }

                try
                {
                    await bookingDetailRepository.Delete(obj);
                    return NoContent();
                }
                catch
                {
                    if (await bookingDetailRepository.GetBookingDetailForAdmin(key) == null)
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
