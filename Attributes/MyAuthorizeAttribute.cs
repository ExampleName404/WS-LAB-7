using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankProductsAPI.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public MyAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Пользователь не аутентифицирован" });
            return;
        }

        if (_roles.Length > 0 && !_roles.Any(user.IsInRole))
        {
            context.Result = new ObjectResult(new { message = "Недостаточно прав для доступа к ресурсу" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
