using Vyapari.Data;
using Vyapari.Infra;
using Vyapari.Service;

namespace Vyapari
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var route = context.Request.Path.Value;
            var method = context.Request.Method;
            var user = context.Items["User"] as UserDto; // Assuming you have a User object in the context

            var whiteListRoutes = new Dictionary<string, List<string>>
            {
                { "/User/signin", new List<string> { "POST" } }, // Add your white-listed routes here
                { "/User/register", new List<string> { "POST" } },
                { "/api/Product", new List<string> { "GET" } },
                { "/api/Product/*", new List<string> { "GET" } }
            };

            var blackListRoutes = new Dictionary<string, Dictionary<string, List<string>>>
            {
                { "/api/Product", new Dictionary<string, List<string>> { { "POST", new List<string> { "admin" } } } }, // Add your black-listed routes here
                { "/api/Product/*", new Dictionary<string, List<string>> { { "POST", new List<string> { "admin" } } } }
            };

            if (route != null && whiteListRoutes.Any(wr => route.StartsWith(wr.Key) && wr.Value.Contains(method)))
            {
                await _next(context);
            }
            else if (blackListRoutes.Any(br => route.StartsWith(br.Key) && br.Value.ContainsKey(method)))
            {
                var allowedRolesForRouteAndMethod = blackListRoutes.First(br => route.StartsWith(br.Key) && br.Value.ContainsKey(method)).Value[method];

                if (allowedRolesForRouteAndMethod.Contains(user.Role))
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access denied");
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized access");
            }
        }

        // public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        // {
        //     using (var scope = serviceProvider.CreateScope())
        //     {
        //         var routeService = scope.ServiceProvider.GetRequiredService<IRouteService>();

        //         var route = context.Request.Path.Value;
        //         var user = context.Items["User"] as UserDto; // Assuming you have a User object in the context

        //         if (route != null && routeService.IsWhiteListed(route))
        //         {
        //             await _next(context);
        //         }
        //         else if (routeService.IsBlackListed(route))
        //         {
        //             var blackListedRoute = await routeService.GetBlackListedRoute(route);
        //             if (blackListedRoute.AllowedRoles != null && blackListedRoute.AllowedRoles.Any(role => role.Name == user.Role))
        //             {
        //                 await _next(context);
        //             }
        //             else
        //             {
        //                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //                 await context.Response.WriteAsync("Access denied");
        //             }
        //         }
        //         else
        //         {
        //             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //             await context.Response.WriteAsync("Unauthorized access");
        //         }
        //     }
        // }
    }
}