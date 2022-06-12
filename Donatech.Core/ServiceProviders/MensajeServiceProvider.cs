using System;
using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
	public class MensajeServiceProvider: IMensajeServiceProvider
	{
		private readonly DonatechDBContext _dbContext;

		public MensajeServiceProvider(DonatechDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ResultDto<List<MensajeDto>>> GetListaMensajesByFilter(FilterMensajeDto filter)
        {
            try
            {
                var mensajes = await _dbContext.Mensajes
                        .Include("IdEmisorNavigation")
                        .Include("IdReceptorNavigation")
                        .Where(m => m.IdProducto == filter.IdProducto)
                        .Select(m =>
                        new MensajeDto
                        {
                            Id = m.Id,
                            FchEnvio = m.FchEnvio,
                            IdEmisor = m.IdEmisor,
                            IdProducto = m.IdProducto,
                            Enabled = m.Enabled,
                            IdReceptor = m.IdReceptor,
                            Mensaje = m.Mensaje1,
                            DatosEmisor = new UsuarioDto
                            {
                                Nombre = m.IdEmisorNavigation.Nombre,
                                Apellidos = m.IdEmisorNavigation.Apellidos
                            },
                            DatosReceptor = new UsuarioDto
                            {
                                Nombre = m.IdReceptorNavigation.Nombre,
                                Apellidos = m.IdReceptorNavigation.Apellidos
                            }
                        })
                        .OrderBy(m => m.FchEnvio)
                        .ToListAsync();

                foreach (var mensaje in mensajes)
                {
                    mensaje.SesionEmisor = mensaje.IdEmisor == filter.UsuarioSession;
                    mensaje.DatosEmisor!.Iniciales = $"{mensaje.DatosEmisor.Nombre[0]}{mensaje.DatosEmisor.Apellidos[0]}".ToUpper();
                    mensaje.DatosReceptor!.Iniciales = $"{mensaje.DatosReceptor.Nombre[0]}{mensaje.DatosReceptor.Apellidos[0]}".ToUpper();
                }

                Console.WriteLine($"Cantidad Mensajes: {mensajes?.Count ?? 0}");

                return new ResultDto<List<MensajeDto>>(mensajes);
            }
			catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new ResultDto<List<MensajeDto>>(error: new ResultError($"Error inesperado al obtener la lista de mensajes", ex));
            }
        }

        public async Task<ResultDto<bool>> InsertMessage(MensajeDto mensaje)
        {
            try
            {
                _dbContext.Mensajes.Add(new Mensaje
                {
                    Id = 0,
                    FchEnvio = mensaje.FchEnvio,
                    IdEmisor = mensaje.IdEmisor,
                    IdProducto = mensaje.IdProducto,
                    IdReceptor = mensaje.IdReceptor,
                    Mensaje1 = mensaje.Mensaje,
                    Enabled = true
                });

                var dbResult = await _dbContext.SaveChangesAsync();

                return dbResult > 0 ?
                    new ResultDto<bool>(true) :
                    new ResultDto<bool>(error: new ResultError("Error al intentar registrar el mensaje. Por favor, intentelo nuevamente"));

            }
            catch (Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError($"Error al intentar registrar el mensaje. Detalle: \"{ex.Message}\"", ex));
            }
        }
    }
}

