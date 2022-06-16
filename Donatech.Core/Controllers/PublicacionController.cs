using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    [JwtAuthorize]
    public class PublicacionController : Controller
    {
        private readonly string cPrefix = "PublicacionController";
        private readonly ILogger _logger;        
        private readonly IProductoServiceProvider _productoServiceProvider;
        private readonly ICommonServiceProvider _commonServiceProvider;

        public PublicacionController(ILogger<PublicacionController> logger,
            IProductoServiceProvider productoServiceProvider,
            ICommonServiceProvider commonServiceProvider)
        {
            _logger = logger;
            _productoServiceProvider = productoServiceProvider;
            _commonServiceProvider = commonServiceProvider;
        }

        // GET: /<controller>/
        [HttpGet]        
        public async Task<IActionResult> MisPublicaciones()
        {
            string mPrefix = "[MisPublicaciones()]";

            try
            {
                var currentUser = JwtSessionUtils.GetCurrentUserSession(HttpContext);
                var result = await _productoServiceProvider.GetProductosByFilter(new Model.FilterProductoDto
                {
                    IdOferente = currentUser?.Id
                });

                if (result.HasError)
                {
                    _logger.AddCustomLog(cPrefix,
                      mPrefix,
                      result.Error!.ErrorMessage!,
                      result.Error?.Exception);

                    ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error al intentar obtener la lista de publicaciones");

                    return View(new PublicacionListViewModel());
                }

                return View(new PublicacionListViewModel
                {
                    PublicacionList = result.Result!
                });
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new PublicacionListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> MiHistorial()
        {
            string mPrefix = "[MisPublicaciones()]";

            try
            {
                var currentUser = JwtSessionUtils.GetCurrentUserSession(HttpContext);
                var result = await _productoServiceProvider.GetProductosByFilter(new Model.FilterProductoDto
                {
                    IdDemandante = currentUser?.Id
                });

                if (result.HasError)
                {
                    _logger.AddCustomLog(cPrefix,
                      mPrefix,
                      result.Error!.ErrorMessage!,
                      result.Error?.Exception);

                    ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error al intentar obtener el historial");

                    return View(new PublicacionListViewModel());
                }

                return View(new PublicacionListViewModel
                {
                    PublicacionList = result.Result!
                });
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new PublicacionListViewModel());
            }
        }

        [HttpGet]
        public IActionResult Buscar()
        {
            return View(new PublicacionListViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(PublicacionListViewModel model)
        {
            string mPrefix = "[Buscar(PublicacionListViewModel model)]";

            try
            {                
                var publicaciones = await _productoServiceProvider.GetProductosByText(model.TextSearch);

                if(publicaciones.Result != null)
                {
                    model.PublicacionList = publicaciones.Result;
                }
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DetallePublicacion(int id)
        {
            string mPrefix = "[DetallePublicacion(int id)]";

            try
            {
                var publicacion = await _productoServiceProvider.GetDetalleProductoById(id);

                if (publicacion.Result != null)
                {
                    return View(new DetallePublicacionViewModel{
                       DetalleProducto = publicacion.Result!
                    });
                }
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");
            }

            return View(new DetallePublicacionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> DetallePublicacion(DetallePublicacionViewModel viewModel)
        {
            string mPrefix = "[DetallePublicacion(int idProducto)]";

            try
            {
                Console.WriteLine($"DetallePublicacion: {viewModel.DetalleProducto.Id}");

                var userSession = JwtSessionUtils.GetCurrentUserSession(HttpContext)!;

                ProductoDto producto = new ProductoDto
                {
                    Id = viewModel.DetalleProducto.Id,
                    IdDemandante = userSession.Id,
                    FchFinalizacion = DateTime.Now
                };

                var result = await _productoServiceProvider.AceptarDonacion(producto);

                if(result.HasError)
                {
                    throw result.Error!.Exception!;
                }

                viewModel.DetalleContacto = result.Result;

                var publicacion = await _productoServiceProvider.GetDetalleProductoById(viewModel.DetalleProducto.Id);

                if (publicacion.Result != null)
                {
                    viewModel.DetalleProducto = publicacion.Result!;
                }

                return View("DetallePublicacion", viewModel);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View("DetallePublicacion", viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            string mPrefix = "[Nuevo()]";

            try
            {
                var lstEstados = _commonServiceProvider.GetListaEstados();
                var lstTipoProductos = await _commonServiceProvider.GetListaTipoProductos();

                if (lstTipoProductos.HasError)
                {
                    throw lstTipoProductos.Error!.Exception!;
                }

                return View(new NuevaPublicacionViewModel
                {
                    EstadoList = lstEstados,
                    TipoProductoList = lstTipoProductos.Result!,
                    PublicacionRegistrada = null
                });
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new NuevaPublicacionViewModel
                {
                    EstadoList = new List<string>(),
                    TipoProductoList = new List<TipoProductoDto>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(NuevaPublicacionViewModel viewModel)
        {
            string mPrefix = "[Nuevo(NuevaPublicacionViewModel viewModel)]";

            try
            {
                var lstEstados = _commonServiceProvider.GetListaEstados();
                var lstTipoProductos = await _commonServiceProvider.GetListaTipoProductos();

                if (lstTipoProductos.HasError)
                {
                    throw lstTipoProductos.Error!.Exception!;
                }

                viewModel.EstadoList = lstEstados;
                viewModel.TipoProductoList = lstTipoProductos.Result!;

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var producto = new ProductoDto();
                producto.Titulo = viewModel.Titulo;
                producto.Descripcion = viewModel.Descripcion;
                producto.Estado = viewModel.Estado;
                producto.FchPublicacion = DateTime.Now;
                producto.IdOferente = JwtSessionUtils.GetCurrentUserSession(HttpContext)!.Id;
                producto.IdDemandante = null;
                producto.IdTipo = viewModel.IdTipo;
                producto.Imagen = viewModel.Imagen;
                producto.ImagenMimeType = viewModel.ImagenMimeType;
                producto.Enabled = true;

                var result = await _productoServiceProvider.CreateProducto(producto);

                if (result.HasError)
                {
                    throw result.Error!.Exception!;
                }

                viewModel.PublicacionRegistrada = result.Result;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // En caso de obtener una excepción inesperada, mostramos un mensaje al usuario
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);

                ModelState.AddModelError("PublicacionesNotFound", "Ha ocurrido un error inesperado");

                return View(new NuevaPublicacionViewModel
                {
                    EstadoList = new List<string>(),
                    TipoProductoList = new List<TipoProductoDto>()
                });
            }
        }
    }
}

