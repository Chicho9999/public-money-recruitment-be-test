using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalService _rentalService;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IRentalService rentalService)
        {
            _rentals = rentals;
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            ValidateRental(rentalId);

            return _rentals[rentalId];
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(RentalBindingModel rentalBinding)
        {
            var newResource = await _rentalService.AddRentalAsync(rentalBinding);

            return newResource;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Update(int rentalId, [FromBody] RentalBindingModel rentalModel)
        {
            ValidateRental(rentalId);

            if (rentalModel.PreparationTimeInDays <= 0)
                throw new ApplicationException("Preparation time in days must be positive");

            var updatedRental = await _rentalService.UpdateRentalAsync(rentalId, rentalModel);

            return updatedRental;
        }

        private void ValidateRental(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
        }
    }
}
