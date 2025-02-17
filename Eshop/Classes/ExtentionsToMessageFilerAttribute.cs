using Eshop.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eshop.Classes
{
    public class ExtentionsToMessageFilerAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception is null)
                return;

            ((Controller)context.Controller).AddDebugMessage(context.Exception);

            string referer = context.HttpContext.Request.Headers["Referer"];
            context.Result = !string.IsNullOrWhiteSpace(referer) ?
                new RedirectResult(referer) : new RedirectToActionResult("Index", "Home", null);

            context.ExceptionHandled = true;
        }
    }
}
