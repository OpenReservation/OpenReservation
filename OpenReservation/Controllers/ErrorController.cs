using Microsoft.AspNetCore.Mvc;
using OpenReservation.WorkContexts;

namespace OpenReservation.Controllers;

public class ErrorController(ILogger<ErrorController> logger) : FrontBaseController(logger)
{
    /// <summary>
    /// 404
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult NotFound(string errorPath)
    {
        Response.StatusCode = 404;
        return View();
    }
}
