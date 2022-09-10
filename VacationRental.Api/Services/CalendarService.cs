using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public class CalendarService : ICalendarService
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        
        public CalendarService(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
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
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= calendarDate.Date && booking.Start.AddDays(booking.Nights) > calendarDate.Date)
                    {
                        calendarDate.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                calendar.Dates.Add(calendarDate);
            }

            return calendar;
        }
    }
}
