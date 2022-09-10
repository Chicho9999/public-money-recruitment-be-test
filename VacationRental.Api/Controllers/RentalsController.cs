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
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(RentalBindingModel rentalBinding)
        {
            var newResource = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };
            await _rentalService.AddRentalAsync(rentalBinding, newResource);

            return newResource;
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Update(int rentalId, [FromBody] RentalBindingModel rentalBinding)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var updatedRental = await _rentalService.UpdateRental(rentalId, rentalBinding);

            return updatedRental;
        }
    }
}
