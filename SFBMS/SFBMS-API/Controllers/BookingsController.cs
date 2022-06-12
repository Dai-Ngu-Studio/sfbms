using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ODataController
    {
        private readonly IBookingRepository bookingRepository;

        public BookingsController(IBookingRepository _bookingRepository)
        {
            bookingRepository = _bookingRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Booking>>> Get()
        {
            return Ok(await bookingRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Booking>> GetSingle([FromODataUri] int key)
        {
            var obj = await bookingRepository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> Post(Booking obj)
        {
            try
            {
                await bookingRepository.Add(obj);
                return Created(obj);
            }
            catch
            {
                if (await bookingRepository.Get(obj.Id) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Booking>> Put(int key, Booking obj)
        {
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await bookingRepository.Update(obj);
                return Ok(obj);
            }
            catch
            {
                if (await bookingRepository.Get(obj.Id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Booking>> Delete(int key)
        {
            try
            {
                await bookingRepository.Delete(key);
                return NoContent();
            }
            catch
            {
                if (await bookingRepository.Get(key) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
