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
                    item.ImagenBase64 = $"data:{item.ImagenMimeType};base64, {(item?.Imagen != null ? Convert.ToBase64String(item?.Imagen!) : "")}";
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

        public async Task<ResultDto<List<ProductoDto>>> GetProductosByText(string text)
        {
            try
            {
                var publicacionesList = await _dbContext.Productos.Include("IdTipoNavigation")
                    .Include("IdOferenteNavigation").Include("IdOferenteNavigation.IdComunaNavigation").Where(p =>
                    ((p.Descripcion.ToLower().Contains(text) || string.IsNullOrEmpty(text)) ||
                    (p.Titulo.ToLower().Contains(text) || string.IsNullOrEmpty(text)) ||
                    (p.IdTipoNavigation.Descripcion.ToLower().Contains(text) || string.IsNullOrEmpty(text))) &&
                    (p.Enabled == true) && (p.FchFinalizacion == null)
                    ).OrderByDescending(p => p.FchPublicacion)
                    .Select(p =>
                    new ProductoDto
                    {
                        Id = p.Id,
                        Descripcion = p.Descripcion,
                        Estado = p.Estado,
                        FchFinalizacion = p.FchFinalizacion,
                        FchPublicacion = p.FchPublicacion,
                        IdOferente = p.IdOferente,
                        Oferente = new UsuarioDto
                        {
                            Apellidos = p.IdOferenteNavigation.Apellidos,
                            Celular = p.IdOferenteNavigation.Celular,
                            Direccion = p.IdOferenteNavigation.Direccion,
                            Email = p.IdOferenteNavigation.Email,
                            Id = p.IdOferenteNavigation.Id,
                            IdComuna = p.IdOferenteNavigation.IdComuna,
                            Comuna = new ComunaDto
                            {
                                Id = p.IdOferenteNavigation.IdComuna,
                                Nombre = p.IdOferenteNavigation.IdComunaNavigation.Nombre ?? ""
                            },
                            IdRol = p.IdOferenteNavigation.IdRol,
                            Nombre = p.IdOferenteNavigation.Nombre,
                            Run = p.IdOferenteNavigation.Run
                        },
                        IdDemandante = p.IdDemandante,
                        Imagen = p.Imagen,
                        ImagenMimeType = p.ImagenMimeType,
                        IdTipo = p.IdTipo,
                        TipoProducto = new TipoProductoDto
                        {
                            Id = p.IdTipo,
                            Descripcion = p.IdTipoNavigation.Descripcion ?? ""
                        },
                        Titulo = p.Titulo,
                        Enabled = p.Enabled
                    }).ToListAsync();

                foreach (var item in publicacionesList)
                {
                    item.ImagenBase64 = $"data:{item.ImagenMimeType};base64, {Convert.ToBase64String(item.Imagen!)}";
                    item.Imagen = null;
                    item.ImagenMimeType = null;
                }

                return new ResultDto<List<ProductoDto>>(result: publicacionesList);
            }
            catch (Exception ex)
            {                
                return new ResultDto<List<ProductoDto>>(error: new ResultError($"Error inesperado al obtener la lista de publicaciones", ex));
            }
        }

        public async Task<ResultDto<ProductoDto>> GetDetalleProductoById(int id)
        {
            try
            {
                var item = await _dbContext.Productos.Include("IdTipoNavigation")
                    .Include("IdOferenteNavigation")
                    .Include("IdOferenteNavigation.IdComunaNavigation")
                    .Where(p => p.Id == id)
                    .OrderByDescending(p => p.FchPublicacion)
                    .Select(p =>
                    new ProductoDto
                    {
                        Id = p.Id,
                        Descripcion = p.Descripcion,
                        Estado = p.Estado,
                        FchFinalizacion = p.FchFinalizacion,
                        FchPublicacion = p.FchPublicacion,
                        IdOferente = p.IdOferente,
                        Oferente = new UsuarioDto
                        {
                            Apellidos = p.IdOferenteNavigation.Apellidos,
                            Celular = p.IdOferenteNavigation.Celular,
                            Direccion = p.IdOferenteNavigation.Direccion,
                            Email = p.IdOferenteNavigation.Email,
                            Id = p.IdOferenteNavigation.Id,
                            IdComuna = p.IdOferenteNavigation.IdComuna,
                            Comuna = new ComunaDto
                            {
                                Id = p.IdOferenteNavigation.IdComuna,
                                Nombre = p.IdOferenteNavigation.IdComunaNavigation.Nombre ?? ""
                            },
                            IdRol = p.IdOferenteNavigation.IdRol,
                            Nombre = p.IdOferenteNavigation.Nombre,
                            Run = p.IdOferenteNavigation.Run
                        },
                        IdDemandante = p.IdDemandante,
                        Imagen = p.Imagen,
                        ImagenMimeType = p.ImagenMimeType,
                        IdTipo = p.IdTipo,
                        TipoProducto = new TipoProductoDto
                        {
                            Id = p.IdTipo,
                            Descripcion = p.IdTipoNavigation.Descripcion ?? ""
                        },
                        Titulo = p.Titulo,
                        Enabled = p.Enabled
                    }).FirstOrDefaultAsync();

                item.ImagenBase64 = $"data:{item.ImagenMimeType};base64, {Convert.ToBase64String(item.Imagen!)}";
                item.Imagen = null;
                item.ImagenMimeType = null;

                return new ResultDto<ProductoDto>(result: item);
            }
            catch (Exception ex)
            {
                return new ResultDto<ProductoDto>(error: new ResultError($"Error inesperado al obtener el detalle de la publicación", ex));
            }
        }

        public async Task<ResultDto<UsuarioDto>> AceptarDonacion(ProductoDto producto)
        {
            try
            {
                var productoDb = await _dbContext.Productos
                    .FirstOrDefaultAsync(p => p.Id == producto.Id);
                productoDb.IdDemandante = producto.IdDemandante;
                productoDb.FchFinalizacion = producto.FchFinalizacion;

                var infoDonante = await _dbContext.Usuarios
                    .Include("IdComunaNavigation")
                    .Where(u => u.Id == productoDb.IdOferente)
                    .Select(u =>
                    new UsuarioDto
                    {
                        Id = u.Id,
                        Apellidos = u.Apellidos,
                        Celular = u.Celular,
                        Comuna = new ComunaDto
                        {
                            Id = u.IdComunaNavigation.Id,
                            Nombre = u.IdComunaNavigation.Nombre
                        },
                        Direccion = u.Direccion,
                        Email = u.Email,
                        IdComuna = u.IdComuna,
                        Nombre = u.Nombre,
                        IdRol = u.IdRol,
                        Run = u.Run
                    }).FirstOrDefaultAsync();

                var dbResult = await _dbContext.SaveChangesAsync();

                return new ResultDto<UsuarioDto>(dbResult > 0 ? infoDonante : null);                    
            }
            catch(Exception ex)
            {
                return new ResultDto<UsuarioDto>(error: new ResultError($"Error inesperado al aceptar la donación", ex));
            }
        }

        public async Task<ResultDto<bool>> CreateProducto(ProductoDto producto)
        {
            try
            {
                _dbContext.Productos.Add(new Producto
                {
                    Descripcion = producto.Descripcion,
                    Estado = producto.Estado,
                    Titulo = producto.Titulo,
                    FchFinalizacion = null,
                    FchPublicacion = producto.FchPublicacion,
                    IdDemandante = null,
                    IdOferente = producto.IdOferente,
                    IdTipo = producto.IdTipo,
                    Imagen = producto.Imagen,
                    ImagenMimeType = producto.ImagenMimeType,
                    Enabled = true
                });

                var dbResult = await _dbContext.SaveChangesAsync();

                return new ResultDto<bool>(dbResult > 0);
            }
            catch(Exception ex)
            {
                return new ResultDto<bool>(error: new ResultError($"Error inesperado al registrar el producto", ex));
            }
        }
    }
}

