using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MainApplicationService.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, CurrentUserProvider currentUserProvider)
        {
            if (httpContext.Request.Headers.TryGetValue("UserId", out var userId))
            {
                currentUserProvider.SetCurrentUser(new User()
                {
                    Id = userId.ToString()
                });
            }
            else
            {
                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsync("Missing required header 'UserId'");
                return;
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
}
