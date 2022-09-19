using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> CheckAvailability(BookingBindingModel newBooking)
        {
            for (var i = 0; i < newBooking.Nights; i++)
            {
                var count = 0;
                foreach (var currentBooking in _bookings.Values)
                {
                    var newBookingEnd = newBooking.Start.AddDays(newBooking.Nights);

                    if (currentBooking.RentalId == newBooking.RentalId
                        && currentBooking.Start <= newBooking.Start.Date && currentBooking.End > newBooking.Start.Date
                        || (currentBooking.Start < newBookingEnd && currentBooking.End >= newBookingEnd)
                        || (currentBooking.Start > newBooking.Start && currentBooking.End < newBookingEnd)
                        )
                    {
                        count++;
                    }
                }

                var rental = await Task.Run(() => _rentals[newBooking.RentalId]);

                if (count >= rental.Units)
                    return false;
            }
            return true;
        }

        public async Task<ResourceIdViewModel> AddBookingAsync(BookingBindingModel newBooking)
        {
            var newResource = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
            var rental = _rentals[newBooking.RentalId];

            await Task.Run( ()=> _bookings.Add(newResource.Id, new BookingViewModel
            {
                Id = newResource.Id,
                Nights = newBooking.Nights,
                RentalId = newBooking.RentalId,
                Start = newBooking.Start.Date,
                End = newBooking.Start.Date.AddDays(newBooking.Nights + rental.PreparationTimeInDays)
            }));

            return newResource;
        }

        public async Task<BookingViewModel> GetBookingAsync(int bookingId)
        {
            var booking = await Task.Run(() => _bookings[bookingId]);

            return booking;
        }
    }
}
