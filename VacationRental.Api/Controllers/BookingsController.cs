using System;
using System.Collections.Generic;
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
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IBookingService _bookingService;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IBookingService bookingService)
        {
            _rentals = rentals;
            _bookings = bookings;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(BookingBindingModel bookingBindingModel)
        {
            if (bookingBindingModel.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if (!_rentals.ContainsKey(bookingBindingModel.RentalId))
                throw new ApplicationException("Rental not found");


            var response = _bookingService.CheckAvailability(bookingBindingModel);

            if (!response)
            {
                throw new ApplicationException("Not available");
            }

            var resource = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            await _bookingService.AddBookingAsync(bookingBindingModel, resource);

            return resource;
        }
    }
}
