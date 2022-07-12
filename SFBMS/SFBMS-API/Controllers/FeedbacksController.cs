using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository userRepository;

        public FeedbacksController(IFeedbackRepository _feedbackRepository, IFieldRepository _fieldRepository, IUserRepository _userRepository)
        {
            feedbackRepository = _feedbackRepository;
            fieldRepository = _fieldRepository;
            userRepository = _userRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Feedback>>> Get()
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var feedbackList = await feedbackRepository.GetList();
                return Ok(feedbackList);
            }
            return Ok(await feedbackRepository.GetUserFeedbacks(GetCurrentUID()));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var admin = await feedbackRepository.Get(key);
                if (admin == null)
                {
                    return NotFound("Feedback not found"); ;
                }
                return Ok(admin);
            }
            var obj = await feedbackRepository.GetSingleUserFeedback(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Feedback not found"); ;
            }
            return Ok(obj);
        }

        //[HttpGet("user-feedbacks")]
        //[EnableQuery(MaxExpansionDepth = 5)]
        //public async Task<ActionResult<List<Feedback>>> GetUserFeedbacks()
        //{
        //    return Ok(await feedbackRepository.GetUserFeedbacks(GetCurrentUID()));
        //}

        //[EnableQuery]
        //[HttpGet("user-feedback/{key}")]
        //public async Task<ActionResult<Feedback>> GetSingleUserFeedback(int key)
        //{
        //    var obj = await feedbackRepository.GetSingleUserFeedback(key, GetCurrentUID());
        //    if (obj == null)
        //    {
        //        return NotFound("Feedback not found"); ;
        //    }
        //    return Ok(obj);
        //}

        [HttpPost]
        public async Task<ActionResult<Feedback>> Post(Feedback obj)
        {
            try
            {
                // Check if field exists
                var field = await fieldRepository.Get(obj.FieldId);
                if (field == null)
                {
                    return NotFound("Field not found");
                }

                // Validate rating
                int userRating = obj.Rating > 5 ? 5
                    : obj.Rating < 0 ? 0
                    : obj.Rating;

                // Create feedback
                var feedback = new Feedback
                {
                    UserId = GetCurrentUID(),
                    FieldId = field.Id,
                    Title = obj.Title,
                    Content = obj.Content,
                    Rating = userRating,
                    FeedbackTime = DateTime.Now,
                    BookingDetailId = obj.BookingDetailId
                };
                await feedbackRepository.Add(feedback);

                // Update total rating
                var feedbacks = await feedbackRepository.GetFieldFeedbacks(obj.FieldId);
                if (feedbacks.Count > 0)
                {
                    var _field = new Field
                    {
                        Id = field.Id,
                        Name = field.Name,
                        Description = field.Description,
                        CategoryId = field.CategoryId,
                        Price = field.Price,
                        NumberOfSlots = field.NumberOfSlots,
                        TotalRating = GetTotalRating(feedbacks),
                        ImageUrl = field.ImageUrl
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
            try
            {
                // Check if feedback exists
                var currentFeedback = await feedbackRepository.Get(key);
                if (currentFeedback == null)
                {
                    return NotFound("Feedback not found");
                }

                // Check if field exists
                var field = await fieldRepository.Get(currentFeedback.FieldId);
                if (field == null)
                {
                    return NotFound("Field not found");
                }

                // Update feedback
                Feedback feedback = new Feedback
                {
                    Id = currentFeedback.Id,
                    FieldId = currentFeedback.FieldId,
                    UserId = currentFeedback.UserId,
                    BookingDetailId = currentFeedback.BookingDetailId,
                    Title = obj.Title == null ? currentFeedback.Title : obj.Title,
                    Content = obj.Content == null ? currentFeedback.Content : obj.Content,
                    Rating = obj.Rating > 5 ? 5 : obj.Rating < 0 ? 0 : obj.Rating,
                    FeedbackTime = DateTime.Now,
                };
                await feedbackRepository.Update(feedback);

                // Update total rating
                var feedbacks = await feedbackRepository.GetFieldFeedbacks(obj.FieldId);
                if (feedbacks.Count > 0)
                {
                    var _field = new Field
                    {
                        Id = field.Id,
                        Name = field.Name,
                        Description = field.Description,
                        CategoryId = field.CategoryId,
                        Price = field.Price,
                        NumberOfSlots = field.NumberOfSlots,
                        TotalRating = GetTotalRating(feedbacks),
                        ImageUrl = field.ImageUrl
                    };
                    await fieldRepository.Update(_field);
                }

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
            try
            {
                // Check if feedback exists
                var obj = await feedbackRepository.Get(key);
                if (obj == null)
                {
                    return NotFound("Feedback not found");
                }

                // Check if field exists
                var field = await fieldRepository.Get(obj.FieldId);
                if (field == null)
                {
                    return NotFound("Field not found");
                }

                // Delete feedback
                await feedbackRepository.Delete(obj);

                // Update total rating
                var feedbacks = await feedbackRepository.GetFieldFeedbacks(obj.FieldId);
                if (feedbacks.Count > 0)
                {
                    var _field = new Field
                    {
                        Id = field.Id,
                        Name = field.Name,
                        Description = field.Description,
                        CategoryId = field.CategoryId,
                        Price = field.Price,
                        NumberOfSlots = field.NumberOfSlots,
                        TotalRating = GetTotalRating(feedbacks),
                        ImageUrl = field.ImageUrl
                    };
                    await fieldRepository.Update(_field);
                }

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
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        private double GetTotalRating(List<Feedback> feedbacks)
        {
            int ratings = 0;
            foreach (var item in feedbacks)
            {
                ratings += item.Rating;
            }
            double totalRating = ratings / feedbacks.Count;
            return totalRating;
        }
    }
}
