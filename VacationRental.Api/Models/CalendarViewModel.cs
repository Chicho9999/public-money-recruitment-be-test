using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
        public Booking Bookings { get; set; }
        public PreparationTimes PreparationTimes { get; set; }
    }
}
