using AuthECapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthECapi.Controllers
{
    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/signup",
    async (
        UserManager<AppUser> userManager,
        [FromBody] UserRegistrationModel userRegistrationModel
        ) =>
    {
        AppUser appUser = new AppUser()
        {
            UserName = userRegistrationModel.Email,
            Email = userRegistrationModel.Email,
            FullName = userRegistrationModel.FullName,
        };
        var result = await userManager.CreateAsync(appUser, userRegistrationModel.Password);
        if (result.Succeeded)
            return Results.Ok(result);
        else
            return Results.BadRequest(result);
    });

            app.MapPost("/signin", async (UserManager<AppUser> userManager,
                [FromBody] LoginModel userLoginModel, IOptions<AppSetting> options) =>
            {
                var user = await userManager.FindByEmailAsync(userLoginModel.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, userLoginModel.Password))
                {
                    var siginKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(options.Value.JWTSecret));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                    new Claim("UserID",user.Id.ToString())
                            }),
                        Expires = DateTime.UtcNow.AddDays(10),
                        SigningCredentials = new SigningCredentials(
                            siginKey,
                            SecurityAlgorithms.HmacSha256Signature
                            )
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Results.Ok(new { token });
                }
                else
                    return Results.BadRequest(new { message = "Username or password is incorrect." });
            });
            return app;
        }
    }
    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
