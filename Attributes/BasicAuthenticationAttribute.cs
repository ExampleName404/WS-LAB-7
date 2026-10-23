using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BankProductsAPI.Services;

namespace BankProductsAPI.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthenticationAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
{
    private const string Scheme = "Basic ";

    public int Order { get; set; } = -1;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var header = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(header) || !header.StartsWith(Scheme, StringComparison.OrdinalIgnoreCase))
        {
            Reject(context, "Отсутствует заголовок Authorization со схемой Basic");
            return;
        }

        string credentials;
        try
        {
            credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header[Scheme.Length..].Trim()));
        }
        catch (FormatException)
        {
            Reject(context, "Некорректная строка Base64");
            return;
        }

        var separator = credentials.IndexOf(':');
        if (separator < 0)
        {
            Reject(context, "Учётные данные должны быть в формате логин:пароль");
            return;
        }

        var userName = credentials[..separator];
        var password = credentials[(separator + 1)..];

        var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
        var user = userService.Validate(userName, password);
        if (user == null)
        {
            Reject(context, "Неверный логин или пароль");
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email)
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, "Basic");
        context.HttpContext.User = new ClaimsPrincipal(identity);
    }

    private static void Reject(AuthorizationFilterContext context, string message)
    {
        context.Result = new UnauthorizedObjectResult(new { message });
    }
}
