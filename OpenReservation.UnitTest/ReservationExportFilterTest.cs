using OpenReservation.AdminLogic;
using OpenReservation.Models;
using Xunit;

namespace OpenReservation.UnitTest;

public class ReservationExportFilterTest
{
    [Theory]
    [InlineData("2026-01-01", "2026-01-31", "2026-01-01", true)]
    [InlineData("2026-01-01", "2026-01-31", "2026-01-31", true)]
    [InlineData("2026-01-01", "2026-01-31", "2025-12-31", false)]
    [InlineData("2026-01-01", "2026-01-31", "2026-02-01", false)]
    public void Create_AppliesInclusiveDateRange(string begin, string end, string reservationDate, bool expected)
    {
        var predicate = ReservationExportFilter.Create(DateTime.Parse(begin), DateTime.Parse(end)).Compile();

        Assert.Equal(expected, predicate(new Reservation { ReservationForDate = DateTime.Parse(reservationDate) }));
    }

    [Fact]
    public void Create_WithoutBounds_IncludesEveryReservation()
    {
        var predicate = ReservationExportFilter.Create(null, null).Compile();

        Assert.True(predicate(new Reservation { ReservationForDate = DateTime.MinValue }));
        Assert.True(predicate(new Reservation { ReservationForDate = DateTime.MaxValue }));
    }
}
