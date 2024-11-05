using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Models;

namespace UserService.Jwt;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateJwt(User user,IList<string> roles)
    {
        var userClaims = new List<Claim>()
        {
            new Claim("Id", user.Id),
            new Claim("UserName", user.UserName)
        };
        
        userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var SecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:JwtSecret"]));
        var credentials = new SigningCredentials(SecKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            userClaims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return token;
    }
}