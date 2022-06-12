using System;
using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
	public class ProductoServiceProvider: IProductoServiceProvider
	{
		private readonly DonatechDBContext _dbContext;

		public ProductoServiceProvider(DonatechDBContext dbContext)
		{
			_dbContext = dbContext;
		}

        public async Task<ResultDto<List<ProductoDto>>> GetProductosByFilter(FilterProductoDto filter)
        {
            try
            {
                var publicacionesList = await _dbContext.Productos.Where(p =>
                        (filter.IdDemandante == null || p.IdDemandante == filter.IdDemandante) &&
                        (filter.IdOferente == null || p.IdOferente == filter.IdOferente) &&
                        (filter.Enabled == null || p.Enabled == filter.Enabled)
                        ).Select(p =>
                    new ProductoDto
                    {
                        Id = p.Id,
                        Descripcion = p.Descripcion,
                        Estado = p.Estado,
                        FchFinalizacion = p.FchFinalizacion,
                        FchPublicacion = p.FchPublicacion,
                        IdOferente = p.IdOferente,
                        IdDemandante = p.IdDemandante,
                        Imagen = p.Imagen,
                        ImagenMimeType = p.ImagenMimeType,
                        IdTipo = p.IdTipo,
                        Titulo = p.Titulo,
                        Enabled = p.Enabled
                    }).ToListAsync();                

                int index = 0;
                foreach (var item in publicacionesList)
                {
                    item.ImagenBase64 = $"{item.ImagenMimeType},{(item?.Imagen != null ? Convert.ToBase64String(item?.Imagen!) : "")}";
                    item.Imagen = null!;
                    item.ImagenMimeType = null!;
                    item.UrlContacto = $"/Contacto/Mensajes?idProducto={item.Id}&idUsuario={item.IdDemandante}";
                    item.Index = index;
                    item.CardDeckHeaderHtml = index == 0 || index % 3 == 0 ? "<div class=\"card-deck\">" : "";
                    item.CardDeckFooterHtml = ((index + 1) % 3 == 0) || (index + 1 == publicacionesList.Count) ? "</div>" : "";
                    index++;
                }

                return new ResultDto<List<ProductoDto>>(result: publicacionesList);
            }
            catch (Exception ex)
            {
                return new ResultDto<List<ProductoDto>>(error: new ResultError($"Error inesperado al obtener la lista de publicaciones", ex));
            }
        }
    }
}

