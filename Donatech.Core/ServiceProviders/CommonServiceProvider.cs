using System;
using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
	public class CommonServiceProvider: ICommonServiceProvider
	{
		private readonly static string cPrefix = "CommonServiceProvider";
		private readonly ILogger _logger;
		private readonly DonatechDBContext _dbContext;

		public CommonServiceProvider(ILogger<CommonServiceProvider> logger,
			DonatechDBContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

        public async Task<ResultDto<List<ComunaDto>>> GetListaComunas()
        {
			try
			{
				var comunas = await (from c in _dbContext.Comunas
							   select new ComunaDto
                               {
								   Id = c.Id,
								   Nombre = c.Nombre!
                               }).ToListAsync();

				return new ResultDto<List<ComunaDto>>(result: comunas);
			}
			catch(Exception ex)
            {
				return new ResultDto<List<ComunaDto>>(error: new ResultError("Error al intentar obtener las comunas desde DB", ex));
            }			
        }

		public async Task<ResultDto<List<TipoProductoDto>>> GetListaTipoProductos()
        {
			try
            {
				var tipoProductos = await _dbContext.TipoProductos.Select(t =>
					new TipoProductoDto
					{
						Id = t.Id,
						Descripcion = t.Descripcion
					}).ToListAsync();

				return new ResultDto<List<TipoProductoDto>>(tipoProductos);
            }
			catch(Exception ex)
            {
				return new ResultDto<List<TipoProductoDto>>(error: new ResultError("Error al intentar obtener los tipo productos desde DB", ex));
			}
        }

		public List<string> GetListaEstados()
        {
			return new List<string>
			{
				"Nuevo",
				"Usado"
			};
        }

		public async Task AddLogRequestAsync(LogRequestDto logRequest)
        {
			string mPrefix = "[AddLogRequestAsync(LogRequestDto logRequest)]";

			try
            {
				_dbContext.LogRequests.Add(new LogRequest
				{
					FchRequest = logRequest.FchRequest,
					Url = logRequest.Url,
					Username = logRequest.Username
				});

				await _dbContext.SaveChangesAsync();
            }
			catch(Exception ex)
            {
				// En caso de obtener una excepción inesperada, guardamos el valor en el logger
				_logger.AddCustomLog(cPrefix,
						mPrefix,
						"Ha ocurrido un error inesperado",
						ex);
			}
        }
	}
}

