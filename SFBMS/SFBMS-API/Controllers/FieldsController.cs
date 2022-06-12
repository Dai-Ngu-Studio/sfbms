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

        public FieldsController(IFieldRepository _fieldRepository)
        {
            fieldRepository = _fieldRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Field>>> Get()
        {
            return Ok(await fieldRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Field>> GetSingle([FromODataUri] int key)
        {
            var obj = await fieldRepository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Field>> Post(Field obj)
        {
            try
            {
                await fieldRepository.Add(obj);
                return Created(obj);
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
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await fieldRepository.Update(obj);
                return Ok(obj);
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
            try
            {
                await fieldRepository.Delete(key);
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
