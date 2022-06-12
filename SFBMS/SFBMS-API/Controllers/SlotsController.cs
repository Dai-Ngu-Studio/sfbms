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
    public class SlotsController : ODataController
    {
        private readonly ISlotRepository slotRepository;

        public SlotsController(ISlotRepository _slotRepository)
        {
            slotRepository = _slotRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Slot>>> Get()
        {
            return Ok(await slotRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Slot>> GetSingle([FromODataUri] int key)
        {
            var obj = await slotRepository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Slot>> Post(Slot obj)
        {
            try
            {
                await slotRepository.Add(obj);
                return Created(obj);
            }
            catch
            {
                if (await slotRepository.Get(obj.Id) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Slot>> Put(int key, Slot obj)
        {
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await slotRepository.Update(obj);
                return Ok(obj);
            }
            catch
            {
                if (await slotRepository.Get(obj.Id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Slot>> Delete(int key)
        {
            try
            {
                await slotRepository.Delete(key);
                return NoContent();
            }
            catch
            {
                if (await slotRepository.Get(key) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
