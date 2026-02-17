using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtHelper
{
  private readonly string _secretKey;

  public JwtHelper(string secretKey)
  {
    _secretKey = secretKey;
  }

  /// <summary>
  /// Genera un token JWT per un ruolo e un evento con durata custom
  /// </summary>
  public string GenerateToken(string userId, int eventId, TimeSpan validFor)
  {
    var claims = new[]
    {
            new Claim("userId", userId),
            new Claim("eventId", eventId.ToString())
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "IscrizioniManager",      // chi genera il token
        audience: "IscrizioniClient",     // chi deve usarlo
        claims: claims,
        expires: DateTime.UtcNow.Add(validFor),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
