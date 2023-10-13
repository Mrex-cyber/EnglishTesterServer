using EnglishTesterServer.Auth;
using EnglishTesterServer.Models;
using EnglishTesterServer.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace EnglishTesterServer.Controllers
{
    public class UserController : Controller
    {
        List<User> registeredUsers = new()
        {
            new User("Tom", 25, "tomBenjamin@gmail.com", "tomas123"),
            new User("Mandy", 25, "bestcandy@gmail.com", "1sweets1"),
        };
        [HttpPost("/api/signin")]
        public async Task<IResult> SignIn()
        {
            var json = String.Empty;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            UserCredentials user = JsonSerializer.Deserialize<UserCredentials>(json)!;

            if (UserValidator.EmailIsValid(user.email) && UserValidator.PasswordIsValid(user.password))
            {
                if (registeredUsers.Where(u => u.Email == user.email && u.Password == user.password).Count() > 0)
                {
                    return Results.Json(MakeToken(user.email));
                }
                return Results.NotFound();
            }
            return Results.Conflict("Email or password is not valid");            
        }
        public record UserCredentials(string email, string password);
        public string MakeToken(string email)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
