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
        private readonly IFieldRepository fieldRepository;

        public SlotsController(ISlotRepository _slotRepository, IFieldRepository _fieldRepository)
        {
            slotRepository = _slotRepository;
            fieldRepository = _fieldRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Slot>>> Get()
        {
            return Ok(await slotRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Slot>> GetSlot(int key)
        {
            var obj = await slotRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Slot not found");
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Slot>> Post(Slot obj)
        {    
            var field = await fieldRepository.Get(obj.FieldId);
            if (field == null)
            {
                return NotFound("Field not found");
            }

            try
            {
                int slotNumbers = await slotRepository.CountFieldSlots(field.Id);
                Slot slot = new Slot
                {
                    FieldId = field.Id,
                    StartTime = obj.StartTime,
                    EndTime = obj.EndTime,  
                    Status = 0,
                    SlotNumber = slotNumbers + 1
                };
                await slotRepository.Add(slot);

                if (slot != null)
                {
                    Field _field = new Field
                    {
                        Id = field.Id,
                        Name = field.Name,
                        Description = field.Description,
                        Price = field.Price,
                        CategoryId = field.CategoryId,
                        NumberOfSlots = field.NumberOfSlots + 1,
                    };
                    await fieldRepository.Update(_field);
                }
                return Created(slot);
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
            var currentSlot = await slotRepository.Get(key);
            if (currentSlot == null)
            {
                return NotFound("Slot not found");
            }

            try
            {
                Slot slot = new Slot
                {
                    Id = currentSlot.Id,
                    FieldId = obj.FieldId == null ? currentSlot.FieldId : obj.FieldId,
                    StartTime = obj.StartTime == DateTime.MinValue ? currentSlot.StartTime : obj.StartTime,
                    EndTime = obj.EndTime == DateTime.MinValue ? currentSlot.EndTime : obj.EndTime,
                    Status = obj.Status < 0 || obj.Status > 1 ? currentSlot.Status : obj.Status,
                    SlotNumber = currentSlot.SlotNumber,
                };

                await slotRepository.Update(slot);
                return Updated(slot);
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
            var slot = await slotRepository.Get(key);
            if (slot == null)
            {
                return NotFound("Slot not found");
            }

            try
            {
                var field = await fieldRepository.Get(slot.FieldId);
                await slotRepository.Delete(slot);

                if (field == null)
                {
                    return NotFound("Field not found");
                }
                Field _field = new Field
                {
                    Id = field.Id,
                    Name = field.Name,
                    Description = field.Description,
                    Price = field.Price,
                    CategoryId = field.CategoryId,
                    NumberOfSlots = field.NumberOfSlots - 1,
                };
                await fieldRepository.Update(_field);
               
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
