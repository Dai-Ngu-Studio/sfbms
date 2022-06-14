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
    public class CategoriesController : ODataController
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return Ok(await categoryRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Category>> GetCategory(int key)
        {
            var obj = await categoryRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Category not found");
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post(Category obj)
        {
            try
            {
                await categoryRepository.Add(obj);
                return Created(obj);
            }
            catch
            {
                if (await categoryRepository.Get(obj.Id) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Category>> Put(int key, Category obj)
        {
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await categoryRepository.Update(obj);
                return Updated(obj);
            }
            catch
            {
                if (await categoryRepository.Get(obj.Id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Category>> Delete(int key)
        {
            var obj = await categoryRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Category not found");
            }

            try
            {
                await categoryRepository.Delete(obj);
                return NoContent();
            }
            catch
            {
                if (await categoryRepository.Get(key) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
