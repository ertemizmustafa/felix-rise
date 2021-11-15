using Felix.Common.Extensions;
using Felix.Common.Settings;
using Felix.Identity.Api.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Services
{
    public class TokenService : ITokenService
    {

        public readonly IdentitySettings _identitySettings;

        public TokenService(IOptions<IdentitySettings> options)
        {
            _identitySettings = options.Value;
        }

        public async Task<SecurityTokenModel> GeneterateJwtToken(TokenModel tokenModel)
        {
            var secTokenModel = new SecurityTokenModel();

            if (string.IsNullOrEmpty(tokenModel?.Code ?? ""))
                return secTokenModel;

            await Task.Run(() =>
            {
                var encodedKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_identitySettings.Secret)), SecurityAlgorithms.HmacSha256Signature);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, tokenModel.Code),
                    new Claim("FullName",tokenModel.FullName)
                };

                if (tokenModel.Roles.ContainItem())
                    claims.AddRange(tokenModel.Roles.Select(x => new Claim("UserType", x)));

                var key = Encoding.ASCII.GetBytes(_identitySettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _identitySettings.Audience,
                    Issuer = _identitySettings.Issuer,
                    Subject = new ClaimsIdentity(claims.ToArray()),
                    Expires = DateTime.UtcNow.AddHours(_identitySettings.Duration),
                    SigningCredentials = encodedKey,
                };

                secTokenModel = new SecurityTokenModel { SecurityToken = new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor), UserCode = tokenModel.Code };

            });

            return secTokenModel;
            
        }
    }
}
