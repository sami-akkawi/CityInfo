using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controller;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IConfiguration configuration) : ControllerBase
{
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationDto authenticationRequestBody)
    {
        AuthenticationUserDto? user = ValidateUserCredentials(authenticationRequestBody.Username, authenticationRequestBody.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        
        SymmetricSecurityKey securityKey = new(Convert.FromBase64String(configuration["Authentication:SecretForKey"]!));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new()
        {
            new("sub", user.UserId.ToString()),
            new("given_name", user.FirstName),
            new("last_name", user.LastName),
            new("city", user.City),
        };

        JwtSecurityToken jwtSecurityToken = new(
            configuration["Authentication:Issuer"],
            configuration["Authentication:Audience"],
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);
        
        string? tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private AuthenticationUserDto? ValidateUserCredentials(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }
        
        return new AuthenticationUserDto(1, username, "Max", "Miller", "Zurich");
    }
}