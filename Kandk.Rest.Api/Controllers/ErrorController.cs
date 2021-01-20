using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kandk.Rest.Api.Controllers
{
  /// <summary>
  /// Writes unhandled exceptions to the log and delivers a user friendly error message.
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class ErrorController : ControllerBase
  {
    private readonly ILogger<ErrorController> _logger;

    /// <summary>
    /// Creates a new instance of the error controller.
    /// </summary>
    /// <param name="logger">A logger.</param>
    public ErrorController(ILogger<ErrorController> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// Writes a log message and returns user friendly error message.
    /// </summary>
    /// <returns>A user friendly error message.</returns>
    [HttpGet]
    public IActionResult Error()
    {
      var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
      var errorMessage = String.Concat(context?.Error.Message, Environment.NewLine, context?.Error.StackTrace);

      _logger.LogError(errorMessage);

      return Problem();
    }
  }
}
