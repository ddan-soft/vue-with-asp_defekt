using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VueWithASP.Server.Controllers
{
  [Route("[controller]")]
  public class ErrorController : Controller
  {
    [Route("development-error")]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
      if (!hostEnvironment.IsDevelopment())
      {
        return NotFound();
      }

      var exceptionHandlerFeature =
          HttpContext.Features.Get<IExceptionHandlerFeature>()!;

      return Problem(
          detail: exceptionHandlerFeature.Error.StackTrace,
          title: exceptionHandlerFeature.Error.Message);
    }

    [Route("production-error")]
    public IActionResult HandleError() =>
        Problem();

    [HttpPost]
    [Route("client-error")]
    public IActionResult LogClientError() { 
      return Problem();
    }
  }
}
