using System;
using Microsoft.AspNetCore.Mvc;

namespace Vita.Api.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	public class HealthController : Controller
  {

    [HttpGet]
    public ActionResult Get()
    {
      return Json(new
      {
        Health = "Good",
        Timestamp = DateTime.UtcNow.ToShortTimeString()
      });
    }
  }
}