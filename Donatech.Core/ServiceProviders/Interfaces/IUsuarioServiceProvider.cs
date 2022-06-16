using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface IUsuarioServiceProvider
	{
		/// <summary>
        /// Metodo para validar que el Run sea único en DB
        /// </summary>
        /// <param name="run">Run a validar</param>
        /// <returns>True si es único, False en caso que ya exista</returns>
		ResultDto<bool> ValidateExistingRun(string run);
		/// <summary>
        /// Metodo para validar que el Email sea único en DB
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si es único, False en caso qua ya exista</returns>
		ResultDto<bool> ValidateExistingEmail(string email);
        /// <summary>
        /// Metodo para validar el login del usuario usando los parametros
        /// email y contraseña 
        /// </summary>
        /// <param name="usuario">Usuario con las credenciales a validar</param>
        /// <returns>UsuarioDto con todos los datos del usuario o vacío en caso contrario</returns>
        Task<ResultDto<UsuarioDto>> UserLogin(UsuarioDto usuario);
        /// <summary>
        /// Metodo para obtener un usuario por un filtro determinado
        /// </summary>
        /// <param name="usuario">Usuario con las credenciales a validar</param>
        /// <returns>UsuarioDto con todos los datos del usuario o vacío en caso contrario</returns>
        Task<ResultDto<UsuarioDto>> GetUsuarioByFiltro(UsuarioDto usuario);
        /// <summary>
        /// Metodo para obtener un usuario por un id determinado
        /// </summary>
        /// <param name="idUsuario">Id del usuario a obtener</param>
        /// <returns>UsuarioDto con todos los datos del usuario o vacío en caso contrario</returns>
        Task<ResultDto<UsuarioDto>> GetUsuarioById(int idUsuario);
        /// <summary>
        /// Metodo para crear un usuario
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear</param>
        /// <returns>True si el usuario se creo correctamente, False en caso contrario</returns>
        Task<ResultDto<bool>> CreateUsuario(UsuarioDto usuario);
        /// <summary>
        /// Metodo para validar la cuenta de un usuario
        /// </summary>
        /// <param name="token">Token de validacion</param>
        /// <returns>True si la cuenta se activo correctamente, False en caso contrario</returns>
        Task<ResultDto<bool>> ValidateAccount(string token);
    }
}

