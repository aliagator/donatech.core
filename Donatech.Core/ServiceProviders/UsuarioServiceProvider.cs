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
        
        public ResultDto<bool> ValidateExistingEmail(string email)
        {
            try
            {
                var exists = _dbContext.Usuarios.FirstOrDefault(u =>
                    u.Email.ToLower().Equals(email.ToLower()));

                return new ResultDto<bool>(exists == null);
            }
            catch(Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar obtener el Email de los Usuarios", ex));
            }
        }

        public ResultDto<bool> ValidateExistingRun(string run)
        {
            try
            {
                var exists = _dbContext.Usuarios.FirstOrDefault(u =>
                    u.Run.ToLower().Equals(run.ToLower()));

                return new ResultDto<bool>(exists == null);
            }
            catch (Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar obtener el Run de los Usuarios", ex));
            }
        }

        public async Task<ResultDto<UsuarioDto>> UserLogin(UsuarioDto usuario)
        {
            try
            {
                var usuarioDb = await _dbContext.Usuarios.FirstOrDefaultAsync(u =>
                    u.Email.ToLower().Equals(usuario.Email.ToLower()) &&
                    u.Password.Equals(usuario.Password));

                if(usuarioDb == null)
                {
                    return new ResultDto<UsuarioDto>(null, new ResultError("Email y/o Contraseña incorrectos"));
                }

                return new ResultDto<UsuarioDto>(new UsuarioDto
                {
                    Id = usuarioDb.Id,
                    Apellidos = usuarioDb.Apellidos,
                    Celular = usuarioDb.Celular,
                    IdComuna = usuarioDb.IdComuna,
                    Direccion = usuarioDb.Direccion,
                    Email = usuarioDb.Email,
                    Enabled = usuarioDb.Enabled,
                    IdRol = usuarioDb.IdRol,
                    Nombre = usuarioDb.Nombre,
                    Password = usuarioDb.Password,
                    Run = usuarioDb.Run
                });
            }
            catch(Exception ex)
            {
                return new ResultDto<UsuarioDto>(error: new ResultError("Error al intentar obtener el Usuario", ex));
            }
        }

        public async Task<ResultDto<UsuarioDto>> GetUsuarioByFiltro(UsuarioDto usuario)
        {
            try
            {
                var usuarioDb = await _dbContext.Usuarios.FirstOrDefaultAsync(u =>
                     u.Run.ToLower().Equals(usuario.Run.ToLower())
                );                

                if (usuarioDb == null)
                {
                    return new ResultDto<UsuarioDto>(null, new ResultError("Usuario no encontrado en DB"));
                }

                return new ResultDto<UsuarioDto>(new UsuarioDto
                {
                    Id = usuarioDb.Id,
                    Apellidos = usuarioDb.Apellidos,
                    Celular = usuarioDb.Celular,
                    IdComuna = usuarioDb.IdComuna,
                    Direccion = usuarioDb.Direccion,
                    Email = usuarioDb.Email,
                    Enabled = usuarioDb.Enabled,
                    IdRol = usuarioDb.IdRol,
                    Nombre = usuarioDb.Nombre,
                    Password = usuarioDb.Password,
                    Run = usuarioDb.Run
                });
            }
            catch (Exception ex)
            {
                return new ResultDto<UsuarioDto>(error: new ResultError("Error al intentar obtener el Usuario", ex));
            }
        }

        public async Task<ResultDto<UsuarioDto>> GetUsuarioById(int idUsuario)
        {
            try
            {
                var infoDonante = await _dbContext.Usuarios.Include("IdComunaNavigation")
                    .Where(u => u.Id == idUsuario)
                       .Select(u =>
                       new UsuarioDto
                       {
                           Id = u.Id,
                           Apellidos = u.Apellidos,
                           Celular = u.Celular,
                           Comuna = new ComunaDto
                           {
                               Id = u.IdComunaNavigation.Id,
                               Nombre = u.IdComunaNavigation.Nombre!
                           },
                           Direccion = u.Direccion,
                           Email = u.Email,
                           IdComuna = u.IdComuna,
                           Nombre = u.Nombre,
                           IdRol = u.IdRol,
                           Run = u.Run
                       }).FirstOrDefaultAsync();                

                return new ResultDto<UsuarioDto>(infoDonante);
            }
            catch(Exception ex)
            {                
                return new ResultDto<UsuarioDto>(error: new ResultError("Error al intentar obtener el Usuario", ex));
            }
        }
    }
}

