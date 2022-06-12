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
    public class FeedbacksController : ODataController
    {
        private readonly IFeedbackRepository feedbackRepository;

        public FeedbacksController(IFeedbackRepository _feedbackRepository)
        {
            feedbackRepository = _feedbackRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Feedback>>> Get()
        {
            return Ok(await feedbackRepository.GetList());
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Feedback>> GetSingle([FromODataUri] int key)
        {
            var obj = await feedbackRepository.Get(key);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback obj)
        {
            try
            {
                await feedbackRepository.Add(obj);
                return Created(obj);
            }
            catch
            {
                if (await feedbackRepository.Get(obj.Id) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Feedback>> Put(int key, Feedback obj)
        {
            if (key != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                await feedbackRepository.Update(obj);
                return Ok(obj);
            }
            catch
            {
                if (await feedbackRepository.Get(obj.Id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Feedback>> Delete(int key)
        {
            try
            {
                await feedbackRepository.Delete(key);
                return NoContent();
            }
            catch
            {
                if (await feedbackRepository.Get(key) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
