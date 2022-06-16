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
                    u.Password.Equals(usuario.Password) &&
                    u.Validated == true);

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
                    Run = usuarioDb.Run,
                    Validated = usuarioDb.Validated,
                    AccountToken = usuarioDb.AccountToken
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
                    Run = usuarioDb.Run,
                    Validated = usuarioDb.Validated,
                    AccountToken = usuarioDb.AccountToken
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
                           Run = u.Run,
                           Validated = u.Validated,
                           AccountToken = u.AccountToken
                       }).FirstOrDefaultAsync();                

                return new ResultDto<UsuarioDto>(infoDonante);
            }
            catch(Exception ex)
            {                
                return new ResultDto<UsuarioDto>(error: new ResultError("Error al intentar obtener el Usuario", ex));
            }
        }

        public async Task<ResultDto<bool>> CreateUsuario(UsuarioDto usuario)
        {
            try
            {
                _dbContext.Usuarios.Add(new Usuario
                {
                    Apellidos = usuario.Apellidos,
                    Direccion = usuario.Direccion,
                    Email = usuario.Email,
                    IdComuna = usuario.IdComuna,
                    IdRol = usuario.IdRol,
                    Nombre = usuario.Nombre,
                    Password = usuario.Password,
                    Run = usuario.Run,
                    Celular = usuario.Celular,
                    Enabled = true,
                    Validated = usuario.Validated,
                    AccountToken = usuario.AccountToken
                });

                var dbResult = await _dbContext.SaveChangesAsync();
                return new ResultDto<bool>(dbResult > 0);
            }
            catch(Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar crear el Usuario", ex));
            }
        }

        public async Task<ResultDto<bool>> ValidateAccount(string token)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u =>
                    u.AccountToken == token &&
                    u.Validated == false &&
                    u.Enabled == true
                );

                if (usuario == null)
                    return new ResultDto<bool>(false);

                usuario.Validated = true;

                var dbResult = await _dbContext.SaveChangesAsync();
                return new ResultDto<bool>(dbResult > 0);
            }
            catch(Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError("Error al intentar validar la cuenta", ex));
            }
        }
    }
}

