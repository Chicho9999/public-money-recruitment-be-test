using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookingService;

        public BookingsController(
            IRentalService rentalService,
            IBookingService bookingService)
        {
            _rentalService = rentalService;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            var booking = await _bookingService.GetBookingAsync(bookingId);
            if (booking == null)
                throw new ApplicationException("Booking not found");

            return booking;
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(BookingBindingModel bookingBindingModel)
        {
            if (bookingBindingModel.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            var rental = await _rentalService.GetRentalAsync(bookingBindingModel.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var isAvailable = await _bookingService.CheckAvailability(bookingBindingModel);
            if (!isAvailable)
            {
                throw new ApplicationException("Not available");
            }

            var newBooking = await _bookingService.AddBookingAsync(bookingBindingModel);

            return newBooking;
        }
    }
}
