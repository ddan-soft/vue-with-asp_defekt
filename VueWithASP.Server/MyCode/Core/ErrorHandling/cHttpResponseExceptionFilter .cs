using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace VueWithASP.Server.MyCode.Core.ErrorHandling
{
  public class cHttpResponseExceptionFilter : IActionFilter, IOrderedFilter
  {
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.Exception is cHttpResponseException httpResponseException)
      {
        context.Result = new ObjectResult(httpResponseException.Value)
        {
          StatusCode = httpResponseException.StatusCode
        };

        context.ExceptionHandled = true;
      }
    }
  }
}
