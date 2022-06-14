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
    public class FeedbacksController : ODataController
    {
        private readonly IFeedbackRepository feedbackRepository;
        private readonly IFieldRepository fieldRepository;

        public FeedbacksController(IFeedbackRepository _feedbackRepository, IFieldRepository _fieldRepository)
        {
            feedbackRepository = _feedbackRepository;
            fieldRepository = _fieldRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Feedback>>> Get()
        {
            return Ok(await feedbackRepository.GetList(GetCurrentUID()));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int key)
        {
            var obj = await feedbackRepository.Get(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Feedback not found"); ;
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback obj)
        {
            var field = await fieldRepository.Get(obj.FieldId);
            if (field == null)
            {
                return NotFound("Field not found");
            }

            try
            {
                Feedback feedback = new Feedback
                {
                    UserId = GetCurrentUID(),
                    FieldId = field.Id,
                    Title = obj.Title,
                    Content = obj.Content,
                    Rating = obj.Rating
                };

                await feedbackRepository.Add(feedback);
                return Created(feedback);
            }
            catch
            {
                if (await feedbackRepository.Get(obj.Id, GetCurrentUID()) != null)
                {
                    return Conflict();
                }
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<Feedback>> Put(int key, Feedback obj)
        {
            var currentFeedback = await feedbackRepository.Get(key, GetCurrentUID());
            if (currentFeedback == null)
            {
                return NotFound("Feedback not found");
            }

            try
            {
                Feedback feedback = new Feedback
                {
                    Id = currentFeedback.Id,
                    FieldId = obj.FieldId == null ? currentFeedback.FieldId : obj.FieldId,
                    UserId = currentFeedback.UserId,
                    Title = obj.Title == null ? currentFeedback.Title : obj.Title,
                    Content = obj.Content == null ? currentFeedback.Content : obj.Content,
                    Rating = obj.Rating < 0 ? currentFeedback.Rating : obj.Rating,
                };

                await feedbackRepository.Update(feedback);
                return Updated(feedback);
            }
            catch
            {
                if (await feedbackRepository.Get(obj.Id, GetCurrentUID()) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<Feedback>> Delete(int key)
        {
            var obj = await feedbackRepository.Get(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Feedback not found");
            }

            try
            {
                await feedbackRepository.Delete(obj);
                return NoContent();
            }
            catch
            {
                if (await feedbackRepository.Get(key, GetCurrentUID()) == null)
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
