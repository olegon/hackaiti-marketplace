using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Cart.Service.API.Exceptions;

namespace Cart.Service.API.Infrastructure.Filters
{
    public class BusinessExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)    
        {
            if (!context.ExceptionHandled)
            {
                var businessException = context.Exception as BusinessException;
                if (businessException != null)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        ErrorMessage = businessException.Message
                    });
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}