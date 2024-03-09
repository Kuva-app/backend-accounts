using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable enable
namespace Kuva.Accounts.Service.Controllers.Filters
{
    public class AuthorizeApiTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var header = context.HttpContext.Request.Headers[Constants.HeaderTokenKey];
            if (header.Count > 0)
            {
                var apiToken = header[0] ?? "";
                if (!string.IsNullOrEmpty(apiToken) && !string.IsNullOrWhiteSpace(apiToken))
                {
                    // TODO: Verificar o token de api no (REDIS OU MONGODB) ?
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
            base.OnActionExecuting(context);
        }
    }
}