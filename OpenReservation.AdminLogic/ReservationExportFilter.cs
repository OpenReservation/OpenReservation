using System.Linq.Expressions;
using OpenReservation.Models;
using WeihanLi.Extensions;

namespace OpenReservation.AdminLogic;

public static class ReservationExportFilter
{
    public static Expression<Func<Reservation, bool>> Create(DateTime? beginDate, DateTime? endDate)
    {
        Expression<Func<Reservation, bool>> predicate = _ => true;
        if (beginDate.HasValue)
        {
            predicate = predicate.And(reservation => reservation.ReservationForDate >= beginDate);
        }
        if (endDate.HasValue)
        {
            predicate = predicate.And(reservation => reservation.ReservationForDate <= endDate);
        }

        return predicate;
    }
}
