using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenReservation.Helpers;
using OpenReservation.WorkContexts;

namespace OpenReservation.WechatAPI.Controllers;

public class WechatConfigController(ILogger<WechatConfigController> logger, OperLogHelper operLogHelper)
    : AdminBaseController(logger, operLogHelper)
{
    public ActionResult Index() => View();
}
