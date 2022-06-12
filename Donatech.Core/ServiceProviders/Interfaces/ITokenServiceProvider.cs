using System;
using Donatech.Core.Model;
using Microsoft.IdentityModel.Tokens;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface ITokenServiceProvider
	{
		/// <summary>
        /// Metodo para generar un token JWT con los parametros necesarios
        /// </summary>
        /// <param name="key">Key del Jwt</param>
        /// <param name="issuer">Issuer del Jwt</param>
        /// <param name="usuario">Datos del Usuario a almacenar en el Jwt</param>
        /// <returns>String con los datos del JWT Token</returns>
		string BuildToken(string key, string issuer, UsuarioDto usuario);
        /// <summary>
        /// Metodo para validar un token JWT que va a venir en un request HTTP
        /// </summary>
        /// <param name="key">Key del Jwt</param>
        /// <param name="issuer">Issuer del Jwt</param>
        /// <param name="audience">Audience del Jwt</param>
        /// <param name="token">Token a validar</param>
        /// <returns>True si el token es válido, False en caso contrario</returns>
		(bool, SecurityToken?) ValidateToken(string key, string issuer, string audience, string token);
	}
}

