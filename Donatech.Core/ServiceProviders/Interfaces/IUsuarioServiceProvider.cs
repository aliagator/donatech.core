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
	}
}

