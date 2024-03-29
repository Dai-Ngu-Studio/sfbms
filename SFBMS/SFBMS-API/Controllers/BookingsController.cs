﻿using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interfaces;
using SFBMS_API.BusinessModels;
using System.Security.Claims;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ODataController
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingDetailRepository bookingDetailRepository;
        private readonly IFieldRepository fieldRepository;
        private readonly ISlotRepository slotRepository;
        private readonly IUserRepository userRepository;

        public BookingsController(IBookingRepository _bookingRepository, IBookingDetailRepository _bookingDetailRepository,
            IFieldRepository _fieldRepository, ISlotRepository _slotRepository, IUserRepository _userRepository)
        {
            bookingRepository = _bookingRepository;
            bookingDetailRepository = _bookingDetailRepository;
            fieldRepository = _fieldRepository;
            slotRepository = _slotRepository;
            userRepository = _userRepository;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 5)]
        public async Task<ActionResult<List<Booking>>> Get()
        {
            return Ok(await bookingRepository.GetList(GetCurrentUID()));
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<ActionResult<Booking>> GetBooking(int key)
        {
            var obj = await bookingRepository.Get(key, GetCurrentUID());
            if (obj == null)
            {
                return NotFound("Booking not found");
            }
            return Ok(obj);
        }

        /// <summary>
        /// Slot must include: id, startTime, endTime, fieldId, slotNumber, status
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Booking>> Post([FromBody] BookingModel bookingModel)
        {
            try
            {
                if (bookingModel.Slots?.Count > 0)
                {
                    var numberOfFields = new HashSet<int>();
                    var bookingDetails = new List<BookingDetail>();
                    decimal totalPrice = 0;
                    foreach (var item in bookingModel.Slots)
                    {
                        if (item.FieldId == null)
                        {
                            continue;
                        }

                        var field = await fieldRepository.Get(item.FieldId);

                        if (field != null)
                        {
                            BookingDetail bookingDetail = new BookingDetail
                            {
                                StartTime = item.StartTime,
                                EndTime = item.EndTime,
                                FieldId = field.Id,
                                UserId = GetCurrentUID(),
                                SlotNumber = item.SlotNumber,
                                Price = field.Price,
                                Status = (int)BookingDetailStatus.NotYet,
                            };
                            bookingDetails.Add(bookingDetail);
                            totalPrice += bookingDetail.Price;
                            numberOfFields.Add(field.Id);
                        }
                    }

                    Booking booking = new Booking
                    {
                        UserId = GetCurrentUID(),
                        TotalPrice = totalPrice,
                        BookingDate = DateTime.Now,
                        NumberOfFields = numberOfFields.Count,
                        BookingDetails = bookingDetails
                    };

                    await bookingRepository.Add(booking);
                    return Created(booking);
                }

                return BadRequest();
            }
            catch
            {
                //if (await bookingRepository.Get(bookingModel.Id, GetCurrentUID()) != null)
                //{
                //    return Conflict();
                //}
                return BadRequest();
            }
        }

        //[HttpPut("{key}")]
        //public async Task<ActionResult<Booking>> Put(int key, Booking obj)
        //{
        //    if (key != obj.Id)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        await bookingRepository.Update(obj);
        //        return Updated(obj);
        //    }
        //    catch
        //    {
        //        if (await bookingRepository.Get(obj.Id, GetCurrentUID()) == null)
        //        {
        //            return NotFound();
        //        }
        //        return BadRequest();
        //    }
        //}

        [HttpDelete("{key}")]
        public async Task<ActionResult<Booking>> Delete(int key)
        {
            User? user = await userRepository.Get(GetCurrentUID());
            if (user != null && user.IsAdmin == 1)
            {
                var obj = await bookingRepository.Get(key, GetCurrentUID());
                if (obj == null)
                {
                    return NotFound("Booking not found");
                }
                int userBookings = await bookingDetailRepository.CountBookingDetails(obj.Id);
                if (userBookings > 0)
                {
                    return BadRequest();
                }

                try
                {
                    await bookingRepository.Delete(obj);
                    return NoContent();
                }
                catch
                {
                    if (await bookingRepository.Get(key, GetCurrentUID()) == null)
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
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
    }
}
