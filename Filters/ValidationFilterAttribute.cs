using Holdy.Holdy.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Holdy.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(ErrorHelper.ModelStateErrorHandler(context.ModelState));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
