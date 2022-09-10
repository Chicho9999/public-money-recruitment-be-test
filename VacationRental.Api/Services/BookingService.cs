using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public async Task AddBookingAsync(BookingBindingModel bookingBindingModel, ResourceIdViewModel resource)
        {
            await Task.Run( ()=> _bookings.Add(resource.Id, new BookingViewModel
            {
                Id = resource.Id,
                Nights = bookingBindingModel.Nights,
                RentalId = bookingBindingModel.RentalId,
                Start = bookingBindingModel.Start.Date
            }));
        }

        public bool CheckAvailability(BookingBindingModel bookingBinding)
        {
            for (var i = 0; i < bookingBinding.Nights; i++)
            {   
                var count = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == bookingBinding.RentalId
                        && (booking.Start <= bookingBinding.Start.Date && booking.Start.AddDays(booking.Nights) > bookingBinding.Start.Date)
                        || (booking.Start < bookingBinding.Start.AddDays(bookingBinding.Nights) && booking.Start.AddDays(booking.Nights) >= bookingBinding.Start.AddDays(bookingBinding.Nights))
                        || (booking.Start > bookingBinding.Start && booking.Start.AddDays(booking.Nights) < bookingBinding.Start.AddDays(bookingBinding.Nights)))
                    {
                        count++;
                    }
                }
                if (count >= _rentals[bookingBinding.RentalId].Units)
                    return false;
            }
            return true;
        }
    }
}
