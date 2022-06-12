using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface IProductoServiceProvider
	{
		Task<ResultDto<List<ProductoDto>>> GetProductosByFilter(FilterProductoDto filter);
	}
}

