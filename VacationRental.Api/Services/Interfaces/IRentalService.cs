using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        Task AddRentalAsync(RentalBindingModel model, ResourceIdViewModel newResource);
        Task<RentalViewModel> UpdateRental(int rentalId, RentalBindingModel rentalBinding);
    }
}
