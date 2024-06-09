using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BookStore.Presentation.Services;

public class ClientAuthenticationService(HttpClient httpClient, IConfiguration _configuration)
{
    public async Task<string> LoginAsync(string username, string password)
    {
        var loginData = new { Username = username, Password = password };
        StringContent content = new(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7092/api/Authentication/login", content);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        JwtToken? token = JsonSerializer.Deserialize<JwtToken>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return token!.Token;
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        catch
        {
            return null;
        }
    }

    private class JwtToken
    {
        public string Token { get; set; } = string.Empty;
    }
}
