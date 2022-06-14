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
    public class FieldsController : ODataController
    {
        private readonly IFieldRepository fieldRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISlotRepository slotRepository;

        public FieldsController(IFieldRepository _fieldRepository, ICategoryRepository _categoryRepository, ISlotRepository _slotRepository)
        {
            fieldRepository = _fieldRepository;
            categoryRepository = _categoryRepository;
            slotRepository = _slotRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Field>>> Get([FromQuery] string? search)
        {
            return Ok(await fieldRepository.GetList(search));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Field>> GetField(int key, [FromQuery] DateTime? date)
        {
            if (date.HasValue)
            {
                var slots = await fieldRepository.GetFieldSlotsByDate(key, date);
                return Ok(slots);
            }
            var obj = await fieldRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Field not found");
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Field>> Post(Field obj)
        {
            var category = await categoryRepository.Get(obj.CategoryId);
            if (category == null)
            {
                return NotFound("Category not found");
            }


            try
            {
                Field field = new Field
                {
                    CategoryId = obj.CategoryId,
                    Name = obj.Name,
                    Description = obj.Description,
                    Price = obj.Price < 0 ? 10000 : obj.Price,
                    NumberOfSlots = 0
                };

                await fieldRepository.Add(field);
                if (obj.Slots?.Count > 0)
                {
                    foreach (var item in obj.Slots)
                    {
                        int slotNumbers = await slotRepository.CountFieldSlots(field.Id);
                        Slot slot = new Slot
                        {
                            FieldId = field.Id,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Status = item.Status,
                            SlotNumber = slotNumbers + 1,
                        };
                        await slotRepository.Add(slot);                      
                    }

                    int fieldSlots = await slotRepository.CountFieldSlots(field.Id);
                    Field _field = new Field
                    {
                        Id = field.Id,
                        CategoryId = field.CategoryId,
                        Name = field.Name,
                        Description = field.Description,
                        Price = field.Price,
                        NumberOfSlots = fieldSlots
                    };
                    await fieldRepository.Update(_field);
                    return Created(_field);
                }

                return Created(field);
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

        [HttpPut("{key}")]
        public async Task<ActionResult<Field>> Put(int key, Field obj)
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
                    Price = obj.Price <= 0 ? currentField.Price : obj.Price,
                    NumberOfSlots = currentField.NumberOfSlots,
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

        [HttpDelete("{key}")]
        public async Task<ActionResult<Field>> Delete(int key)
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
    }
}
