using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        Task<ResourceIdViewModel> AddRentalAsync(RentalBindingModel newRental);
        Task<RentalViewModel> UpdateRentalAsync(int rentalId, RentalBindingModel rentalBinding);
        Task<RentalViewModel> GetRentalAsync(int rentalId);
    }
}
