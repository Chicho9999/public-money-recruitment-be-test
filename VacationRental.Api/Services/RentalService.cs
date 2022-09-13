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
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public RentalService(IDictionary<int, RentalViewModel> rentals, IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public async Task<ResourceIdViewModel> AddRentalAsync(RentalBindingModel newRental)
        {
            var newResource = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            await Task.Run(() => _rentals.Add(newResource.Id, new RentalViewModel
            {
                Id = newResource.Id,
                Units = newRental.Units,
                PreparationTimeInDays = newRental.PreparationTimeInDays
            }));

            return newResource;
        }

        public async Task<RentalViewModel> UpdateRentalAsync(int rentalId, RentalBindingModel rentalBinding)
        {
            var rentalToUpdate = await Task.Run(() => _rentals[rentalId]);

            var bookings = _bookings.Values.Where(b => b.RentalId == rentalId);

            var diffInDays = rentalBinding.PreparationTimeInDays - rentalToUpdate.PreparationTimeInDays;

            var overlapBooking = 0;
            
            for (int i = 1; i < bookings.Count(); i++)
            {
                var currentBooking = bookings.FirstOrDefault(x => x.Id == i);
                var otherBookings = bookings.Where(b => b.Id != i && currentBooking.End <= b.Start.Date);

                if (otherBookings.Any(o => currentBooking.End.AddDays(diffInDays) >= o.Start.Date))
                {
                    overlapBooking++;
                }

                var overlap = bookings.Count(b => currentBooking.Start < b.End && b.Start < currentBooking.End);

                if(overlap > rentalBinding.Units)
                {
                    throw new Exception("Can't reduce the number of units because they are occupied");
                }
            }

            if(overlapBooking >= rentalToUpdate.Units)
            {
                throw new Exception("Can't add more preparation days due to the overlapping of days with others bookings");
            }

            //Update End Date Depending on the new Preparation Time 
            for (int i = 1; i <= bookings.Count(); i++)
            {
                var currentBooking = _bookings[i];
                currentBooking.End = currentBooking.Start.Date.AddDays(currentBooking.Nights + rentalBinding.PreparationTimeInDays);
            }

            rentalToUpdate.Units = rentalBinding.Units;
            rentalToUpdate.PreparationTimeInDays = rentalBinding.PreparationTimeInDays;

            return rentalToUpdate;
        }

        public async Task<RentalViewModel> GetRentalAsync(int rentalId)
        {
            var rental = await Task.Run(() => _rentals[rentalId]);

            return rental;
        }
    }
}
