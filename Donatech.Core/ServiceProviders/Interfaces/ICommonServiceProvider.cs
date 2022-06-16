using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface ICommonServiceProvider
	{
		Task<ResultDto<List<ComunaDto>>> GetListaComunas();
		Task<ResultDto<List<TipoProductoDto>>> GetListaTipoProductos();
		List<string> GetListaEstados();
		Task AddLogRequestAsync(LogRequestDto logRequest);
	}
}

