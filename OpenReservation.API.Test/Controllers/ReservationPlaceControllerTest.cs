using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenReservation.Database;
using OpenReservation.Models;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OpenReservation.API.Test.Controllers;

public class ReservationPlaceControllerTest(APITestFixture fixture) : ControllerTestBase(fixture)
{
    [Fact]
    public async Task GetReservationPlaceList()
    {
        var result = await Client.GetFromJsonAsync<ReservationPlace[]>("/api/v1/reservation-places");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetReservationPlacePeriods()
    {
        using var scope = Services.CreateScope();
        var place = await scope.ServiceProvider.GetRequiredService<ReservationDbContext>()
            .ReservationPlaces.AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(place);

        var result = await Client.GetFromJsonAsync<ReservationPeriod[]>($"/api/v1/reservation-places/{place.PlaceId:N}/availability?date={DateTime.Today:yyyy-MM-dd}");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetReservationPlaceAvailabilityWithInvalidRequest()
    {
        using var response = await Client.GetAsync("/api/v1/reservation-places/00000000-0000-0000-0000-000000000000/availability");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
