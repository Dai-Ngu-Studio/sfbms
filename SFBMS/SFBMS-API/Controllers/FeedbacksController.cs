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
        public async Task<ActionResult<List<Feedback>>> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await feedbackRepository.GetList(page, size));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int key)
        {
            var obj = await feedbackRepository.Get(key);
            if (obj == null)
            {
                return NotFound("Feedback not found"); ;
            }
            return Ok(obj);
        }

        [HttpGet("user-feedbacks")]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Feedback>>> GetUserFeedbacks([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await feedbackRepository.GetUserFeedbacks(GetCurrentUID(), page, size));
        }

        [EnableQuery]
        [HttpGet("user-feedback/{key}")]
        public async Task<ActionResult<Feedback>> GetSingleUserFeedback(int key)
        {
            var obj = await feedbackRepository.GetSingleUserFeedback(key, GetCurrentUID());
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
                int userRating = 0;
                if (obj.Rating < 0)
                {
                    userRating = 0;
                }
                if (obj.Rating > 5)
                {
                    userRating = 5;
                }

                Feedback feedback = new Feedback
                {
                    UserId = GetCurrentUID(),
                    FieldId = field.Id,
                    Title = obj.Title,
                    Content = obj.Content,
                    Rating = userRating
                };
                await feedbackRepository.Add(feedback);

                double feedbackCounts = await feedbackRepository.CountFeedbacks(obj.FieldId);
                List<Feedback> fieldFeedbackList = await feedbackRepository.GetFieldFeedbacks(obj.FieldId);

                if (fieldFeedbackList.Count() > 0)
                {
                    int ratings = 0;
                    foreach (var item in fieldFeedbackList)
                    {
                        ratings += item.Rating;
                    }
                    double totalRating = Math.Round(ratings / feedbackCounts);

                    Field _field = new Field
                    {
                        Id = field.Id,
                        Name = field.Name,
                        Description = field.Description,
                        CategoryId = field.CategoryId,
                        Price = field.Price,
                        NumberOfSlots = field.NumberOfSlots,
                        TotalRating = totalRating
                    };
                    await fieldRepository.Update(_field);
                }

                return Created(feedback);
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
            var currentFeedback = await feedbackRepository.Get(key);
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
            var obj = await feedbackRepository.Get(key);
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
                if (await feedbackRepository.Get(key) == null)
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
