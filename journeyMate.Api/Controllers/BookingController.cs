using AutoMapper;
using journeyMate.Api.Errors;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace journeyMate.Api.Controllers
{
    [Authorize]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingToReturnDto>> CreateBooking([FromBody] BookingDto bookingDto)
        {
            // Validate model state first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var booking = await _bookingService.CreateBookingAsync(buyerEmail, bookingDto);

            if (booking == null)
                return BadRequest(new ErrorResponse(400, "Problem creating booking"));

            return Ok(_mapper.Map<BookingToReturnDto>(booking));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BookingToReturnDto>>> GetBookingForUserById()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var booking = await _bookingService.GetBookingForUserById(buyerEmail);

            if (booking == null)
                return BadRequest(new ErrorResponse(400, "No booking belong to this user"));

            return Ok(_mapper.Map<IEnumerable<BookingToReturnDto>>(booking));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingToReturnDto>> UpdateBooking(int id, BookingDto bookingDto)
        {
            // Validate model state first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatedBooking = await _bookingService.UpdateBookingAsync(id, buyerEmail, bookingDto);

            if (updatedBooking == null)
                return BadRequest(new ErrorResponse(400, "Problem updating booking or booking not found"));

            return Ok(_mapper.Map<BookingToReturnDto>(updatedBooking));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _bookingService.DeleteBookingAsync(id, buyerEmail);

            if (!result)
                return BadRequest(new ErrorResponse(400, "Problem deleting booking or booking not found"));

            return Ok(new { Message = "Booking deleted successfully" });
        }
    }
}