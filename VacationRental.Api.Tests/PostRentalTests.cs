using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGet_ThenAUpdateReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                
                Assert.Equal(request.Units, getResult.Units);
            }

            request.PreparationTimeInDays = 25;

            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.True(putResponse.IsSuccessStatusCode);

                var putResult = await putResponse.Content.ReadAsAsync<RentalViewModel>();
                
                Assert.Equal(request.Units, putResult.Units);
                Assert.Equal(request.PreparationTimeInDays, putResult.PreparationTimeInDays);
            }
        }
    }
}