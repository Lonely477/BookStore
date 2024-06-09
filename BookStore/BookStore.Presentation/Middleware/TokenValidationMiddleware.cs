using BookStore.Presentation.Services;

namespace BookStore.Presentation.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ClientAuthenticationService _authService;

    public TokenValidationMiddleware(RequestDelegate next, ClientAuthenticationService authService)
    {
        _next = next;
        _authService = authService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Session.GetString("JWTToken");
        if (!string.IsNullOrEmpty(token))
        {
            var principal = _authService.ValidateToken(token);
            if (principal is not null)
            {
                context.User = principal;
            }
        }

        await _next(context);
    }
}
