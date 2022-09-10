using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface ICalendarService
    {
        CalendarViewModel RetrieveOcupiedDates(int rentalId, DateTime start, int nights);
    }
}
