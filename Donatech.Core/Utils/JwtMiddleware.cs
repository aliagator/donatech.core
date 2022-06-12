using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.Extensions.Options;

namespace Donatech.Core.Utils
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

		public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
            _configuration = configuration;
		}

        public async Task Invoke(HttpContext context, IUsuarioServiceProvider usuarioServiceProvider, ITokenServiceProvider tokenServiceProvider)
        {
            // Obtenemos el token desde la petición HTTP
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();            

            // Validamos que el token sea correcto
            var tokenValidated = tokenServiceProvider.ValidateToken(
                key: _configuration.GetValue<string>("Jwt:Key"),
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                token: token!);            

            // Verificamos la respuesta del TokenServiceProvider
            if (tokenValidated.Item1)
            {
                // Obtenemos los identities (ClaimsIdentity) desde el jwtToken
                var jwtToken = (JwtSecurityToken)tokenValidated.Item2!;
                var userRun = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                Console.WriteLine($"Middleware: > Token Validated: > userRun: {userRun}");

                // Obtenemos el usuario por el ClaimEmail
                var userDb = await usuarioServiceProvider.GetUsuarioByFiltro(new Model.UsuarioDto
                {
                    Run = userRun
                });

                Console.WriteLine($"Middleware: > Token Validated: > UserDb: {userDb.Result!.NombreCompleto}");

                // Si el usuario existe en DB, lo adjuntamos al HttpContext
                if (!userDb.HasError)
                    context.Session.Set(Constants.UserSessionContextId, userDb.Result!);
            }

            await _next(context);
        }
    }
}

