using BusinessObject;
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
    public class BookingDetailsController : ODataController
    {
        private readonly IBookingDetailRepository bookingDetailRepository;

        public BookingDetailsController(IBookingDetailRepository _bookingDetailRepository)
        {
            bookingDetailRepository = _bookingDetailRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<BookingDetail>>> Get()
        {
            return Ok(await bookingDetailRepository.GetList(GetCurrentUID()));
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

        [HttpPost]
        public async Task<ActionResult<BookingDetail>> Post(BookingDetail obj)
        {
            try
            {
                await bookingDetailRepository.Add(obj);
                return Created(obj);
            }
            catch
            {
                if (await bookingDetailRepository.Get(obj.Id, GetCurrentUID()) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<BookingDetail>> Put(int key, BookingDetail obj)
        {
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await bookingDetailRepository.Update(obj);
                return Ok(obj);
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
