using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace B4.Api.Middleware
{
    public class DisabledEndpointAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new StatusCodeResult(410); // 410 Gone o 403 Forbidden
        }
    }
}