using Microsoft.AspNetCore.Authorization;
using Vyapari.Infra;

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
            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                var authorizeAttribute = endpoint.Metadata.GetOrderedMetadata<AuthAttribute>().FirstOrDefault();

                if (authorizeAttribute != null)
                {
                    var roles = authorizeAttribute.Roles;

                    if (!string.IsNullOrEmpty(roles))
                    {
                        var user = context.Items["User"] as UserDto;

                        if (user == null || !roles.Split(",").Any(r => user.Role.ToLower().Equals(r.ToLower())))
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}