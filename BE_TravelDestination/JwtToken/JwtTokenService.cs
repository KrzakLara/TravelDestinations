using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _signingKey;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        var keyAsString = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyAsString) || keyAsString.Length < 32)
        {
            throw new InvalidOperationException("The JWT signing key must be at least 32 characters long.");
        }

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyAsString));
        if (_signingKey.KeySize < 256)
        {
            throw new InvalidOperationException("The JWT signing key must be at least 256 bits long.");
        }
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
