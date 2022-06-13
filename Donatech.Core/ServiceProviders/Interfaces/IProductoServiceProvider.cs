using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface IProductoServiceProvider
	{
		Task<ResultDto<List<ProductoDto>>> GetProductosByFilter(FilterProductoDto filter);
		Task<ResultDto<List<ProductoDto>>> GetProductosByText(string text);
		Task<ResultDto<ProductoDto>> GetDetalleProductoById(int id);
		Task<ResultDto<UsuarioDto>> AceptarDonacion(ProductoDto producto);
		Task<ResultDto<bool>> CreateProducto(ProductoDto producto);
	}
}

