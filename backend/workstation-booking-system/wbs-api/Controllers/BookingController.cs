using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wbs_api.DTOs;
using wbs_api.Models;
using wbs_api.Repositories.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wbs_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{

    private readonly IBookingRepository _bookingRepository;

    public BookingController(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound(new { Message = $"Booking with ID {id} not found." });
        }

        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingDTO booking)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var targetDate = booking.BookingDate?.Date ?? DateTime.Today;

        var existingBooking = await _bookingRepository.GetActiveBookingAsync(booking.WorkstationId,targetDate);
        if (existingBooking != null)
        {
            return BadRequest(new
            {
                Message = "Workstation is already reserved."
            });
        }

        var existingBookingForUser = await _bookingRepository.GetActiveBookingPerDateAsync(targetDate,booking.UserId);

        if (existingBookingForUser != null)
        {
            return BadRequest(new
            {
                Message = "User already has a booking for this date."
            });
        }

        if (booking.IsPermanent)
        {
            booking.BookingDate = null;
        }
        else
        {
            if (!booking.BookingDate.HasValue)
            {
                return BadRequest(new
                {
                    Message = "Booking date is required for non-permanent bookings."
                });
            }
        }

        Booking newBooking = new Booking
        {
            WorkstationId = booking.WorkstationId,
            UserId = booking.UserId,
            UserName = booking.UserName,
            BookingDate = booking.BookingDate,
            IsPermanent = booking.IsPermanent,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        var createdBooking = await _bookingRepository.CreateAsync(newBooking);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdBooking.Id },
            createdBooking);
    }

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] Booking booking)
    //{
    //    if (booking == null)
    //        return BadRequest("Booking payload is required.");

    //    if (booking.IsPermanent)
    //    {
    //        booking.BookingDate = null;
    //    }
    //    else
    //    {
    //        if (!booking.BookingDate.HasValue)
    //        {
    //            return BadRequest(new
    //            {
    //                Message = "Booking date is required for non-permanent bookings."
    //            });
    //        }
    //    }

    //    booking.CreatedDate = DateTime.UtcNow;
    //    booking.UpdatedDate = DateTime.UtcNow;

    //    var createdBooking = await _bookingRepository.CreateAsync(booking);

    //    return CreatedAtAction(
    //        nameof(GetById),
    //        new { id = createdBooking.Id },
    //        createdBooking);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] Booking booking)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    booking.BookingDate ??= DateTime.UtcNow.Date;
    //    booking.CreatedDate = DateTime.UtcNow;
    //    booking.UpdatedDate = DateTime.UtcNow;

    //    var createdBooking = await _bookingRepository.CreateAsync(booking); createdBooking.UpdatedDate = DateTime.UtcNow;
    //    return CreatedAtAction(nameof(GetById), new { id = createdBooking.Id }, createdBooking);
    //}

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Booking request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound(new { Message = $"Booking with ID {id} not found." });
        }

        booking.WorkstationId = request.WorkstationId;
        booking.UserName = request.UserName;
        booking.BookingDate = request.BookingDate;
        booking.UpdatedDate = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);
        return Ok(new { Message = "Booking updated successfully." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound(new { Message = $"Booking with ID {id} not found." });
        }

        await _bookingRepository.DeleteAsync(booking);

        return Ok(new { Message = "Booking deleted successfully." });
    }

}
