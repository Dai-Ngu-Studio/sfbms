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
    [Authorize]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;

        public CategoriesController(ICategoryRepository _categoryRepository, IUserRepository _userRepository)
        {
            categoryRepository = _categoryRepository;
            userRepository = _userRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var categoryList = await categoryRepository.GetList();
            return Ok(categoryList);
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
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
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
            return Unauthorized();
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Category>> Put(int key, Category obj)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
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
            return Unauthorized();
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Category>> Delete(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
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
            return Unauthorized();
        }
        private string GetCurrentUID()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
