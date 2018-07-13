using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Vita.Api.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	[Produces("application/json")]
	[Route("[controller]")]
  public class VersionController : Controller
  {

    [HttpGet]
    public IActionResult Get()
    {
      Log.Debug("api version {time}", DateTime.UtcNow.ToShortTimeString());
      return Json(new
      {
        Version = AppVersion.Current,
        Timestamp = DateTime.UtcNow.ToShortTimeString()
      });
    }
	}

  public class AppVersion
  {
    // private cache
    private static string _assemblyVersion;

    /// <summary>
    ///     Get the current app version
    /// </summary>
    public static string Current
    {
      get
      {
        if (string.IsNullOrEmpty(_assemblyVersion))
        {
          _assemblyVersion = GetVersionInformation();
        }

        return _assemblyVersion;
      }

      set { _assemblyVersion = value; }
    }

    //helper function that do reflection
    private static string GetVersionInformation()
    {
      var version = Assembly.GetExecutingAssembly().GetName().Version;
      return version.ToString(3);
    }
  }
}