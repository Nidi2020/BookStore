using BookStore.Common.Settings;
using BookStore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Services;

public class JwtService : IJwtService
{
    private readonly IOptionsSnapshot<SiteSettings> _siteSetting;
    private readonly SignInManager<User> signInManager;
    public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager)
    {
        _siteSetting = settings;
        this.signInManager = signInManager;
    }
    public async Task<string> GenerateAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(_siteSetting.Value.JwtSettings.SecretKey); // longer that 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.Value.JwtSettings.Encryptkey); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claims = await _getClaimsAsync(user);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _siteSetting.Value.JwtSettings.Issuer,
            Audience = _siteSetting.Value.JwtSettings.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(_siteSetting.Value.JwtSettings.NotBeforeMinutes),
            Expires = DateTime.Now.AddMinutes(_siteSetting.Value.JwtSettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };


        //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        var jwt = tokenHandler.WriteToken(securityToken);

        return jwt;
    }

    private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
    {
        List<Claim> claims = new();
        var result = await signInManager.ClaimsFactory.CreateAsync(user);

        if (result != null)
            claims = new List<Claim>(result.Claims);

        return claims;
    }
}

