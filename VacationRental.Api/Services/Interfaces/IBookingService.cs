using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingService
    {
        bool CheckAvailability(BookingBindingModel bookingBinding);

        Task AddBookingAsync(BookingBindingModel bookingBindingModel, ResourceIdViewModel resource);
    }
}
