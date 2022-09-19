using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public class CalendarService : ICalendarService
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public CalendarService(IDictionary<int, BookingViewModel> bookings, IDictionary<int, RentalViewModel> rentals)
        {
            _bookings = bookings;
            _rentals = rentals;
        }

        public CalendarViewModel RetrieveOcupiedDates(int rentalId, DateTime start, int nights)
        {
            var calendar = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var calendarDate = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new PreparationTimes()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= calendarDate.Date && booking.Start.AddDays(booking.Nights) > calendarDate.Date)
                    {
                        calendarDate.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Units = _rentals[rentalId].Units });
                        calendarDate.PreparationTimes.Units = _rentals[rentalId].PreparationTimeInDays;
                    }
                }

                calendar.Dates.Add(calendarDate);
            }

            return calendar;
        }
    }
}
