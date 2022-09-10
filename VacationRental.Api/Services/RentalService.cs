using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
        }

        public async Task AddRentalAsync(RentalBindingModel rentalBinding, ResourceIdViewModel newResource)
        {
            await Task.Run(() => _rentals.Add(newResource.Id, new RentalViewModel
            {
                Id = newResource.Id,
                Units = rentalBinding.Units
            }));
        }

        public async Task<RentalViewModel> UpdateRental(int rentalId, RentalBindingModel rentalBinding)
        {
            var rentalToUpdate = await Task.Run( () => _rentals.FirstOrDefault(x => x.Value.Id == rentalId).Key);

            _rentals[rentalToUpdate].Units = rentalBinding.Units;
            _rentals[rentalToUpdate].PreparationTimeInDays = rentalBinding.PreparationTimeInDays;

            return _rentals[rentalToUpdate];
        }
    }
}
