
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JourneyMate.Infrastracture.Repositories;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(string userId, string role)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]??"");
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expires = DateTime.UtcNow.AddHours(1);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    public async Task<string> CreatTokenAsync(AppUser appUser, UserManager<AppUser> userManager)
    {
        //Private Claims 
        var authclims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,appUser.UserName),
                new Claim(ClaimTypes.Email,appUser.Email),
                new Claim(ClaimTypes.NameIdentifier,appUser.Id)

            };
        var Roles = await userManager.GetRolesAsync(appUser);
        foreach (var role in Roles)
        {
            authclims.Add(new Claim(ClaimTypes.Role, role));
        }

        //Create Key 
        var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        //Create Token 

        var Token = new JwtSecurityToken(
            audience: _configuration["JWT:ValidAudience"],
            issuer: _configuration["JWT:ValidIssuer"],
            expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:Duration"])),
            claims: authclims,
            signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
            );
        // generate Token 
        return new JwtSecurityTokenHandler().WriteToken(Token);
    }
}
