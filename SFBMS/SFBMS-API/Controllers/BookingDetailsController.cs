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
    //[Authorize]
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
        public async Task<ActionResult<List<BookingDetail>>> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            //User? user = await userRepository.Get(GetCurrentUID());
            //if (user != null && user.IsAdmin == 1)
            //{
            //    var adminList = await bookingDetailRepository.GetAdminList(page, size);
            //    int TotalBookingDetails = await bookingDetailRepository.GetTotalBookingDetail();
            //    int TotalPages = (TotalBookingDetails - 1) / size + 1;

            //    var model = new
            //    {
            //        bookingDetails = adminList,
            //        numOfBookingDetailPages = TotalPages
            //    };
            //    return Ok(model);
            //}
            var adminList = await bookingDetailRepository.GetAdminList(page, size);
            int TotalBookingDetails = await bookingDetailRepository.GetTotalBookingDetail();
            int TotalPages = (TotalBookingDetails - 1) / size + 1;

            var model = new
            {
                bookingDetails = adminList,
                numOfBookingDetailPages = TotalPages
            };
            return Ok(model);
            //return Ok(await bookingDetailRepository.GetUserList(GetCurrentUID(), page, size));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<BookingDetail>> GetBookingDetail(int key)
        {
            var obj = await bookingDetailRepository.Get(key, GetCurrentUID());
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
            var currentBookingDetail = await bookingDetailRepository.Get(key, GetCurrentUID());
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
                if (await bookingDetailRepository.Get(obj.Id, GetCurrentUID()) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<BookingDetail>> Delete(int key)
        {
            var obj = await bookingDetailRepository.Get(key, GetCurrentUID());
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
                if (await bookingDetailRepository.Get(key, GetCurrentUID()) == null)
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
