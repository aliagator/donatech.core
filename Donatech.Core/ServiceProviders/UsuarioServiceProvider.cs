using System;
using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
	public class UsuarioServiceProvider: IUsuarioServiceProvider
	{
		private readonly DonatechDBContext _dbContext;

		public UsuarioServiceProvider(DonatechDBContext dbContext)
		{
			_dbContext = dbContext;
		}
        
        public async Task<ResultDto<bool>> ValidateExistingEmail(string email)
        {
            try
            {
                var exists = await _dbContext.Usuarios.FirstOrDefaultAsync(u =>
                    u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

                return new ResultDto<bool>(exists == null);
            }
            catch(Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar obtener el Email de los Usuarios", ex));
            }
        }

        public async Task<ResultDto<bool>> ValidateExistingRun(string run)
        {
            try
            {
                var exists = await _dbContext.Usuarios.FirstOrDefaultAsync(u =>
                    u.Run.Equals(run, StringComparison.InvariantCultureIgnoreCase));

                return new ResultDto<bool>(exists == null);
            }
            catch (Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar obtener el Run de los Usuarios", ex));
            }
        }
    }
}

