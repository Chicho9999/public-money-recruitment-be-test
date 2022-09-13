using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingService
    {
        Task<bool> CheckAvailability(BookingBindingModel bookingBinding);
        Task<ResourceIdViewModel> AddBookingAsync(BookingBindingModel bookingBindingModel);
        Task<BookingViewModel> GetBookingAsync(int bookingId);
    }
}
