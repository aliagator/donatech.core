using System;
using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
	public class CommonServiceProvider: ICommonServiceProvider
	{
		private readonly DonatechDBContext _dbContext;

		public CommonServiceProvider(DonatechDBContext dbContext)
		{
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
	}
}

