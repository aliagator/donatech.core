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
		Task<ResultDto<bool>> ValidateExistingRun(string run);
		/// <summary>
        /// Metodo para validar que el Email sea único en DB
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si es único, False en caso qua ya exista</returns>
		Task<ResultDto<bool>> ValidateExistingEmail(string email);
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
    }
}

