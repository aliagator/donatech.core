using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Donatech.Core.ServiceProviders
{
	public class TokenServiceProvider: ITokenServiceProvider
	{        
        private readonly IConfiguration _configuration;

		public TokenServiceProvider(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        public string BuildToken(string key, string issuer, UsuarioDto usuario)
        {
            var claims = new[]
            {                
                new Claim(ClaimTypes.Name, usuario.NombreCompleto.Value),
                new Claim(ClaimTypes.Role, ((EnumUtils.RolEnum)usuario.IdRol).ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Run)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:TokenDurationMinutes")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public (bool, SecurityToken?) ValidateToken(string key, string issuer, string audience, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,                   
                }, out SecurityToken validatedToken);

                return (true, validatedToken);
            }
            catch
            {
                return (false, null);
            }            
        }
    }
}

