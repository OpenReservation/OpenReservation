using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Database;
using OpenReservation.Helpers;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.API;

[Route("/api/v1/reservation-places")]
public class ReservationPlacesController(
    ILogger<ReservationPlacesController> logger,
    IEFRepository<ReservationDbContext, ReservationPlace> repository)
    : ApiControllerBase(logger)
{
    /// <summary>
    /// 预约活动室列表
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await repository.GetResultAsync(p => new
        {
            p.PlaceName,
            p.PlaceIndex,
            p.PlaceId,
            p.MaxReservationPeriodNum
        }, builder => builder
            .WithPredict(x => x.IsActive)
            .WithOrderBy(x => x
                .OrderBy(a => a.PlaceIndex)
                .ThenBy(a => a.UpdateTime)),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// 获取可预约的时间段
    /// </summary>
    /// <param name="placeId">活动室id</param>
    /// <param name="dt">预约日期</param>
    [HttpGet("{placeId}/periods")]
    public IActionResult GetPeriodsAsync(Guid placeId, DateTime dt)
    {
        var result = HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
            .GetAvailablePeriodsByDateAndPlace(dt, placeId);
        return Ok(result);
    }

    /// <summary>
    /// Gets the availability for a reservation place on a date.
    /// </summary>
    [HttpGet("/api/v1/reservation-places/{placeId}/availability")]
    public IActionResult GetAvailability(Guid placeId, [FromQuery] DateTime date)
    {
        if (placeId == Guid.Empty || date == default)
        {
            return BadRequest();
        }

        var result = HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
            .GetAvailablePeriodsByDateAndPlace(date, placeId);
        return Ok(result);
    }
}
