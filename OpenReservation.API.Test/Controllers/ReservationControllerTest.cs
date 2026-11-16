using OpenReservation.Models;
using OpenReservation.ViewModels;
using System.Net;
using System.Net.Http.Json;
using WeihanLi.Common.Models;
using WeihanLi.Common.Services;
using Xunit;

namespace OpenReservation.API.Test.Controllers;

public class ReservationControllerTest(APITestFixture fixture) : ControllerTestBase(fixture)
{
    [Fact]
    public async Task GetReservationList()
    {
        var result = await Client.GetFromJsonAsync<PagedListResult<ReservationListViewModel>>("/api/v1/reservations?pageNumber=1&pageSize=10");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task MakeReservationWithInvalidRequest()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/reservations");
        request.Headers.TryAddWithoutValidation("UserId", GuidIdGenerator.Instance.NewId());
        request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);

        request.Content = JsonContent.Create(new { });

        using var response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task MakeReservationWithUserInfo()
    {
        var places = await Client.GetFromJsonAsync<ReservationPlace[]>("/api/v1/reservation-places");
        var place = Assert.Single(places!, x => x.PlaceName == "第一多功能厅");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/reservations");
        request.Headers.TryAddWithoutValidation("UserId", GuidIdGenerator.Instance.NewId());
        request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);
        request.Headers.TryAddWithoutValidation("UserRoles", "User,ReservationManager");
        request.Headers.TryAddWithoutValidation("captchaType", "None");

        request.Content = JsonContent.Create(new
        {
            reservationUnit = "nnnnn",
            reservationActivityContent = "13211112222",
            reservationPersonName = "谢谢谢",
            reservationPersonPhone = "13211112222",
            reservationPlaceId = place.PlaceId,
            reservationPlaceName = place.PlaceName,
            reservationForDate = TimeProvider.System.GetUtcNow().AddDays(5).ToString("yyyy-MM-dd"),
            reservationForTime = "10:00~12:00",
            reservationForTimeIds = "1"
        });

        using var response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task MakeReservationWithInvalidUserInfo()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/reservations");
        request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);
        request.Content = JsonContent.Create(new
        {
            reservationUnit = "nnnnn",
            reservationActivityContent = "13211112222",
            reservationPersonName = "Test",
            reservationPersonPhone = "13211112222",
            reservationPlaceId = "f9833d13-a57f-4bc0-9197-232113667ece",
            reservationPlaceName = "Place1",
            reservationForDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)).ToString("yyyy-MM-dd"),
            reservationForTime = "10:00~12:00",
            reservationForTimeIds = "1"
        });

        using var response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task MakeReservationWithoutUserInfo()
    {
        using var response = await Client.PostAsJsonAsync("/api/v1/reservations", new
        {
            reservationUnit = "nnnnn",
            reservationActivityContent = "13211112222",
            reservationPersonName = "Test",
            reservationPersonPhone = "13211112222",
            reservationPlaceId = "f9833d13-a57f-4bc0-9197-232113667ece",
            reservationPlaceName = "Place1",
            reservationForDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)).ToString("yyyy-MM-dd"),
            reservationForTime = "10:00~12:00",
            reservationForTimeIds = "1"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
