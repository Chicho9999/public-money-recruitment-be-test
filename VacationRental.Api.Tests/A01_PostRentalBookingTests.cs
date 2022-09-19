using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class A01_PostRentalBookingTests
    {
        private readonly HttpClient _client;

        public A01_PostRentalBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenPostBooking_ReturnsX()
        {
            //Arrange
            var requestRental = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            var secondRental = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 3
            };

            var thirdRental = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            var fourthRental = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 4
            };

            var booking = new BookingBindingModel()
            {
                Nights = 4,
                RentalId = 1,
                Start = new DateTime(2022, 09, 13)
            };

            var anotherBooking = new BookingBindingModel()
            {
                Nights = 5,
                RentalId = 1,
                Start = new DateTime(2022, 09, 21)
            };

            ResourceIdViewModel postResult;
            ResourceIdViewModel postBookingResult;

            //Act
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", requestRental))
            {
                //Assert
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            //Act
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", booking))
            {
                //Assert
                Assert.True(postResponse.IsSuccessStatusCode);
                postBookingResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
                Assert.NotNull(postBookingResult);
            }

            //Act
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", booking))
            {
                //Assert
                Assert.True(postResponse.IsSuccessStatusCode);
                postBookingResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
                Assert.NotNull(postBookingResult);
            }

            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", anotherBooking))
            {
                //Assert
                Assert.True(postResponse.IsSuccessStatusCode);
                postBookingResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
                Assert.NotNull(postBookingResult);
            }

            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", secondRental))
            {
                Assert.True(putResponse.IsSuccessStatusCode);

                var putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
                
                Assert.Equal(secondRental.Units, putResult.Units);
                Assert.Equal(secondRental.PreparationTimeInDays, putResult.PreparationTimeInDays);
            }

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                using (var postBooking2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", thirdRental))
                {
                }
            });

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                using (var postBooking2Response = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", fourthRental))
                {
                }
            });
        }
    }
}