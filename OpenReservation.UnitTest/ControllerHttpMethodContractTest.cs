using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using OpenReservation.AdminLogic.Controllers;
using Xunit;

namespace OpenReservation.UnitTest;

public class ControllerHttpMethodContractTest
{
    [Theory]
    [InlineData(typeof(BlockEntityController), nameof(BlockEntityController.AddEntity))]
    [InlineData(typeof(BlockEntityController), nameof(BlockEntityController.UpdateEntityStatus))]
    [InlineData(typeof(BlockEntityController), nameof(BlockEntityController.DeleteEntity))]
    [InlineData(typeof(DisabledPeriodController), nameof(DisabledPeriodController.UpdatePeriodStatus))]
    [InlineData(typeof(DisabledPeriodController), nameof(DisabledPeriodController.DeletePeriod))]
    [InlineData(typeof(ReservationManageController), nameof(ReservationManageController.DeleteReservation))]
    [InlineData(typeof(ReservationPlaceController), nameof(ReservationPlaceController.AddPlace))]
    [InlineData(typeof(ReservationPlaceController), nameof(ReservationPlaceController.UpdatePlaceStatus))]
    [InlineData(typeof(SystemSettingsController), nameof(SystemSettingsController.UpdateSettings))]
    public void AdminMutations_RequirePost(Type controllerType, string actionName)
    {
        Assert.NotNull(controllerType.GetMethod(actionName)?.GetCustomAttribute<HttpPostAttribute>());
    }

    [Theory]
    [InlineData(nameof(OpenReservation.Controllers.HomeController.GetAvailablePeriods))]
    [InlineData(nameof(OpenReservation.Controllers.HomeController.NoticeList))]
    [InlineData(nameof(OpenReservation.Controllers.HomeController.NoticeDetails))]
    public void PublicReadActions_RequireGet(string actionName)
    {
        var action = typeof(OpenReservation.Controllers.HomeController).GetMethod(actionName);

        Assert.NotNull(action?.GetCustomAttribute<HttpGetAttribute>());
    }
}
