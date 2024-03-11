using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Vyapari.Data;
using Vyapari.Infra;

namespace Vyapari;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AuthenticationMiddleware(RequestDelegate next,IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            using var scope = context.RequestServices.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var user= mapper.Map<UserDto>(await userRepository.GetByIdAsync(userId));

            context.Items["User"] = user;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            // Do nothing if token validation fails
            // User is not attached to context so request won't have access to secure routes
        }
    }
}

// public class AuthenticationMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly IConfiguration _configuration;
//     private readonly IUserRepository _userRepository;
//     private readonly IMapper _mapper;

//     public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration, IUserRepository userRepository, IMapper mapper)
//     {
//         _next = next;
//         _configuration = configuration;
//         _userRepository = userRepository;
//         _mapper = mapper;
//     }

//     public async Task Invoke(HttpContext context)
//     {
//         var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//         if (token != null)
//         {
//             await AttachUserToContext(context, token);
//         }

//         await _next(context);
//     }

//     private async Task AttachUserToContext(HttpContext context, string token)
//     {
//         try
//         {
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
//             tokenHandler.ValidateToken(token, new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(key),
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ClockSkew = TimeSpan.Zero
//             }, out SecurityToken validatedToken);

//             var jwtToken = (JwtSecurityToken)validatedToken;
//             var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

//             context.Items["User"] = await _userRepository.GetByIdAsync(userId);
//         }
//         catch
//         {
//             // Do nothing if token validation fails
//             // User is not attached to context so request won't have access to secure routes
//         }
//     }
// }

//  public class AuthenticationMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly IConfiguration _configuration;

//     public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
//     {
//         _next = next;
//         _configuration = configuration;
//     }

//     public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
//     {
//         var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//         if (token != null)
//         {
//             await AttachUserToContext(context, token, serviceProvider);
//         }

//         await _next(context);
//     }

//     private async Task AttachUserToContext(HttpContext context, string token, IServiceProvider serviceProvider)
//     {
//         using var scope = serviceProvider.CreateScope();
//         var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
//         var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

//         try
//         {
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
//             tokenHandler.ValidateToken(token, new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(key),
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ClockSkew = TimeSpan.Zero
//             }, out SecurityToken validatedToken);

//             var jwtToken = (JwtSecurityToken)validatedToken;
//             var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

//             var user = await userRepository.GetByIdAsync(userId);

//             context.Items["User"] = mapper.Map<UserDto>(user);
//         }
//         catch (Exception ex)
//         {
//             // Log the error or handle it as needed
//             Console.WriteLine($"Token validation failed: {ex.Message}");
//             // User is not attached to context so request won't have access to secure routes
//         }
//     }
// }