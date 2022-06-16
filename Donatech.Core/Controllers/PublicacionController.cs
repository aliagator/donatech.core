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

                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/MisPublicaciones",
                    Username = currentUser?.Email
                });

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

                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/MiHistorial",
                    Username = currentUser?.Email
                });

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
        public async Task<IActionResult> Buscar()
        {
            // Guardamos el log del request
            await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
            {
                FchRequest = DateTime.Now,
                Url = "/Publicacion/MiHistorial",
                Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
            });

            return View(new PublicacionListViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(PublicacionListViewModel model)
        {
            string mPrefix = "[Buscar(PublicacionListViewModel model)]";

            try
            {
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/Buscar",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

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
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/DetallePublicacion",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

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
                var userSession = JwtSessionUtils.GetCurrentUserSession(HttpContext)!;

                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/DetallePublicacion",
                    Username = userSession.Email
                });

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
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/Nuevo",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

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
                // Guardamos el log del request
                await _commonServiceProvider.AddLogRequestAsync(new LogRequestDto
                {
                    FchRequest = DateTime.Now,
                    Url = "/Publicacion/Nuevo",
                    Username = JwtSessionUtils.GetCurrentUserSession(HttpContext)?.Email
                });

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

                if(Request.Form.Files.Count == 0)
                {
                    ModelState.AddModelError("Imagen", "Debe ingresar una Imagen del Producto");
                    return View(viewModel);
                }

                var imageData = Request.Form.Files[0];
                byte[] imageBytes = null;

                using (var stream = new MemoryStream((int)imageData.Length))
                {
                    await imageData.CopyToAsync(stream);
                    imageBytes = stream.ToArray();
                }

                var producto = new ProductoDto();
                producto.Titulo = viewModel.Titulo;
                producto.Descripcion = viewModel.Descripcion;
                producto.Estado = viewModel.Estado;
                producto.FchPublicacion = DateTime.Now;
                producto.IdOferente = JwtSessionUtils.GetCurrentUserSession(HttpContext)!.Id;
                producto.IdDemandante = null;
                producto.IdTipo = viewModel.IdTipo;
                producto.Imagen = imageBytes;
                producto.ImagenMimeType = imageData.ContentType;
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

